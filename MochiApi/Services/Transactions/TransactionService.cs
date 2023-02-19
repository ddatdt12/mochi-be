using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Models;
using static MochiApi.Common.Enum;

namespace MochiApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _configuration;
        private readonly IBudgetService _budgetService;
        private readonly IWalletService _walletService;
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly List<CategoryType> PrivateCategoryTypes = new List<CategoryType>() {
            CategoryType.Debt,
            CategoryType.Repayment,
            CategoryType.Loan,
            CategoryType.DebtCollection
        };
        private readonly List<CategoryType> MinusCategoryTypes = new List<CategoryType>() {
            CategoryType.Expense,
            CategoryType.Repayment,
            CategoryType.Loan,
        };
        private readonly List<CategoryType> PlusCategoryTypes = new List<CategoryType>() {
            CategoryType.Income,
            CategoryType.Debt,
            CategoryType.DebtCollection,
        };
        public DataContext _context { get; set; }

        public TransactionService(IConfiguration configuration, DataContext context, IMapper mapper, IBudgetService budgetService, IWalletService walletService, IEventService eventService)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _budgetService = budgetService;
            _walletService = walletService;
            _eventService = eventService;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(int userId, int? walletId, TransactionFilterDto filter)
        {
            var transQuery = _context.Transactions.AsQueryable();

            if (walletId.HasValue)
            {
                transQuery = transQuery.Where(t => t.WalletId == walletId);
            }
            else
            {
                transQuery = transQuery.Where(t => t.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == MemberStatus.Accepted));
            }

            transQuery = transQuery.Include(t => t.Category)
            .Include(t => t.Event)
            .OrderByDescending(t => t.CreatedAt)
            .AsNoTracking();

            if (filter.CategoryId.HasValue)
            {
                transQuery = transQuery.Where(t => t.CategoryId == filter.CategoryId);
            }

            if (filter.EventId.HasValue)
            {
                transQuery = transQuery.Where(t => t.EventId == filter.EventId);
            }

            if (filter.StartDate.HasValue)
            {
                var startDate = filter.StartDate?.Date;
                transQuery = transQuery.Where(t => t.CreatedAt >= startDate);
            }

            if (filter.EndDate.HasValue)
            {
                var EndDate = filter.EndDate?.Date.AddDays(1).AddSeconds(-1);
                transQuery = transQuery.Where(t => t.CreatedAt <= EndDate);
            }

            if (filter.Skip.HasValue)
            {
                transQuery = transQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                transQuery = transQuery.Take(filter.Take.Value);
            }

            var trans = await transQuery.ToListAsync();
            return trans;
        }

        public async Task<Transaction?> GetTransactionById(int id)
        {
            var transQuery = _context.Transactions.Where(t => t.Id == id)
            .Include(t => t.Category)
            .Include(t => t.Event)
            .Include(t => t.Creator)
            .Include(t => t.Wallet)
            .Include(t => t.RelevantTransaction)
            .OrderByDescending(t => t.CreatedAt)
            .AsNoTracking();

            var trans = await transQuery.FirstOrDefaultAsync();
            if (trans == null)
            {
                return trans;
            }
            var participants = trans.ParticipantIds.Split(";", StringSplitOptions.RemoveEmptyEntries).Where(i => i.All(char.IsDigit)).Select(id => Convert.ToInt32(id))
            .ToList();

            if (participants.Count > 0)
            {
                trans.Participants = await _context.Users.AsNoTracking().Where(u => participants.Contains(u.Id)).ToListAsync();
            }

            return trans;
        }
        public async Task<List<Transaction>> GetChildTransactionsOfParentTrans(int id)
        {
            var parentTran = await _context.Transactions.Where(t => t.Id == id).Include(t => t.Category)
            .AsNoTracking().FirstOrDefaultAsync();
            if (parentTran == null)
            {
                throw new ApiException("Transation not found", 404);
            }

            if (parentTran!.Category!.Type != CategoryType.Debt && parentTran.Category.Type != CategoryType.Loan)
            {
                return new List<Transaction>();
            }
            return await _context.Transactions.Where(t => t.RelevantTransactionId == parentTran.Id)
            .Include(t => t.Category)
            .ToListAsync();
        }

        private async Task HandleDeletePrivateTrans(Transaction trans)
        {

            if (trans.Category == null)
            {
                return;
            }
            Transaction? relevantTrans = null;
            if (trans.RelevantTransactionId.HasValue)
            {
                relevantTrans = await _context.Transactions.Where(t => t.Id == trans.RelevantTransactionId).Include(t => t.Category).FirstOrDefaultAsync();

                if (relevantTrans == null)
                {
                    throw new ApiException("Transaction not found", 400);
                }
            }

            switch (trans.Category.Type)
            {
                case CategoryType.Repayment:
                case CategoryType.DebtCollection:
                    if (relevantTrans != null)
                    {
                        relevantTrans.AccumulatedAmount -= trans.Amount;
                    }
                    break;
            }
        }
        private async Task HandlePrivateTrans(Transaction newTrans, Category? cate = null)
        {
            if (cate == null)
            {
                cate = await _context.Categories.Where(c => c.Id == newTrans.CategoryId).FirstOrDefaultAsync();
                if (cate == null)
                {
                    throw new ApiException("Category not found", 400);
                }
            }

            if (!PrivateCategoryTypes.Contains(cate.Type))
            {
                return;
            }

            Transaction? relevantTrans = null;
            if (newTrans.RelevantTransactionId.HasValue)
            {
                relevantTrans = await _context.Transactions.Where(t => t.Id == newTrans.RelevantTransactionId).Include(t => t.Category).FirstOrDefaultAsync();

                if (relevantTrans == null)
                {
                    throw new ApiException("Transaction not found", 400);
                }
            }
            if (newTrans.UnknownParticipants.Count > 1)
            {
                throw new ApiException("Can't import more people ", 400);
            }
            switch (cate.Type)
            {
                case CategoryType.Debt:
                case CategoryType.Loan:
                    newTrans.RelevantTransactionId = null;
                    break;
                case CategoryType.Repayment:
                    if (relevantTrans != null)
                    {
                        if (relevantTrans.Category!.Type != CategoryType.Debt)
                        {
                            throw new ApiException("Only accept dept transaction", 400);
                        }

                        if (relevantTrans.Amount < relevantTrans.AccumulatedAmount + newTrans.Amount)
                        {
                            throw new ApiException("Unable to pay more than the amount owed. Remain: " + (relevantTrans.Amount - relevantTrans.AccumulatedAmount), 400);
                        }

                        newTrans.UnknownParticipantsStr = relevantTrans.UnknownParticipantsStr;
                        relevantTrans.AccumulatedAmount += newTrans.Amount;
                    }

                    break;
                case CategoryType.DebtCollection:
                    if (relevantTrans != null)
                    {
                        if (relevantTrans.Category!.Type != CategoryType.Loan)
                        {
                            throw new ApiException("Only accept loan transaction! ", 400);
                        }

                        if (relevantTrans.Amount < relevantTrans.AccumulatedAmount + newTrans.Amount)
                        {
                            throw new ApiException("Can't collect an amount greater than the amount you lent! Remain: " + (relevantTrans.Amount - relevantTrans.AccumulatedAmount), 400);
                        }
                        newTrans.UnknownParticipantsStr = relevantTrans.UnknownParticipantsStr;
                        relevantTrans.AccumulatedAmount += newTrans.Amount;
                    }
                    break;
                default:
                    break;
            }
        }
        public async Task<Transaction> CreateTransaction(int userId, int walletId, CreateTransactionDto transDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var trans = _mapper.Map<Transaction>(transDto);

                trans.CreatorId = userId;
                trans.WalletId = walletId;

                if (trans.EventId.HasValue && !await _eventService.CheckEventIsInWallet((int)trans.EventId, trans.WalletId, trans.CreatorId))
                {
                    throw new ApiException("Invalid Event", 400);
                }

                var cate = await _context.Categories.Where(c => c.Id == trans.CategoryId).FirstOrDefaultAsync();
                if (cate == null)
                {
                    throw new ApiException("Category not found", 400);
                }

     
                await HandlePrivateTrans(trans, cate);

                int amount = transDto.Amount;
                if (MinusCategoryTypes.Contains(cate!.Type))
                {
                    amount *= -1;
                }
                await _walletService.UpdateWalletBalance(walletId, amount);
                await _context.Transactions.AddAsync(trans);
                await _context.SaveChangesAsync();

                if (trans.EventId.HasValue)
                {
                    await _eventService.UpdateEventSpent((int)trans.EventId);
                }

                await _budgetService.UpdateSpentAmount(trans.CategoryId, trans.CreatedAt.Month,
                trans.CreatedAt.Year);

                if (transDto.ParticipantIds.Count > 0)
                {
                    var memberIds = await _context.WalletMembers
                    .Where(wM => wM.WalletId == walletId && wM.Status == MemberStatus.Accepted)
                    .Select(wM => wM.UserId).ToListAsync();
                    var invalidUsers = transDto.ParticipantIds
                .Except(memberIds).Select(id => id).ToList();
                    if (invalidUsers.Count > 0)
                    {
                        throw new ApiException("Users " + string.Join(',', invalidUsers) + " not exist!", 400);
                    }
                    trans.ParticipantIds = String.Join(';', transDto.ParticipantIds.Where(id => id != trans.CreatorId).ToList());
                }
                else
                {
                    trans.ParticipantIds = "";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return trans;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateTransaction(int transactionId, int walletId, UpdateTransactionDto updateTransDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var trans = await _context.Transactions
                .Where(t => t.Id == transactionId && t.WalletId == walletId)
                .Include(t => t.Category)
                .FirstOrDefaultAsync();


                if (trans == null)
                {
                    throw new ApiException("Transaction not found!", 400);
                }


                if (PrivateCategoryTypes.Contains(trans!.Category!.Type) && (trans.CategoryId != updateTransDto.CategoryId || trans.UnknownParticipantsStr != string.Join(";", updateTransDto.UnknownParticipants)))
                {
                    throw new ApiException("Cannot update category and participant of this transaction!", 400);
                }

                if (trans.EventId.HasValue && !await _eventService.CheckEventIsInWallet((int)trans.EventId, trans.WalletId, trans.CreatorId))
                {
                    throw new ApiException("Invalid Event", 400);
                }


                var oldTrans = new Transaction
                {
                    Id = trans.Id,
                    Amount = trans.Amount,
                    CategoryId = trans.CategoryId,
                    CreatedAt = trans.CreatedAt,
                    Category = new Category { Id = trans!.Category.Id, Type = trans.Category.Type }
                };

                _mapper.Map(updateTransDto, trans);
                await HandlePrivateTrans(trans, null);

                var memberIds = await _context.WalletMembers
                .Where(wM => wM.WalletId == walletId)
                .Select(wM => wM.UserId).ToListAsync();
                var invalidUsers = updateTransDto.ParticipantIds
                .Except(memberIds).Select(id => id).ToList();
                if (invalidUsers.Count > 0)
                {
                    throw new ApiException("Users " + string.Join(',', invalidUsers) + " not exist!", 400);
                }
                trans.ParticipantIds = String.Join(';', updateTransDto.ParticipantIds.Where(id => id != trans.CreatorId));

                await _context.SaveChangesAsync();

                //Update budget, event after save
                if (updateTransDto.CategoryId != oldTrans.CategoryId)
                {
                    await _budgetService.UpdateSpentAmount(oldTrans.CategoryId, updateTransDto.CreateAtValue.Month,
                    updateTransDto.CreateAtValue.Year);

                    await _budgetService.UpdateSpentAmount(oldTrans.CategoryId, oldTrans.CreatedAt.Month,
                    oldTrans.CreatedAt.Year);

                    var updatedCate = await _context.Categories.Where(c => c.Id == updateTransDto.CategoryId).FirstOrDefaultAsync();
                    if (updatedCate == null)
                    {
                        throw new ApiException("Category not found", 400);
                    }

                    int amount = oldTrans.Amount;
                    int updatedAmount = updateTransDto.Amount;
                    if (MinusCategoryTypes.Contains(updatedCate.Type))
                    {
                        updatedAmount *= -1;
                    }
                    if (PlusCategoryTypes.Contains(oldTrans!.Category!.Type))
                    {
                        amount *= -1;
                    }
                    await _walletService.UpdateWalletBalance(walletId, amount + updatedAmount);
                }
                else if (updateTransDto.Amount != oldTrans.Amount)
                {
                    await _budgetService.UpdateSpentAmount(oldTrans.CategoryId, updateTransDto.CreateAtValue.Month,
            updateTransDto.CreateAtValue.Year);

                    int amount = updateTransDto.Amount - oldTrans.Amount;
                    if (MinusCategoryTypes.Contains(oldTrans.Category!.Type))
                    {
                        amount *= -1;
                    }
                    await _walletService.UpdateWalletBalance(walletId, amount);
                }


                if (trans.EventId.HasValue || updateTransDto.EventId.HasValue)
                {
                    await _eventService.UpdateEventSpent((int)trans.EventId!);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTransaction(int transactionId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var trans = await _context.Transactions.Where(t => t.Id == transactionId).Include(t => t.Category).FirstOrDefaultAsync();
                if (trans == null)
                {
                    throw new ApiException("Transaction not found!", 400);
                }

                if (trans.EventId.HasValue && !(await _eventService.CheckEventIsInWallet((int)trans.EventId, trans.WalletId, trans.CreatorId)))
                {
                    throw new ApiException("Invalid Event", 400);
                }

                await HandleDeletePrivateTrans(trans);


                int amount = trans.Amount;
                if (PlusCategoryTypes.Contains(trans.Category!.Type))
                {
                    amount *= -1;
                }
                await _walletService.UpdateWalletBalance(trans.WalletId, amount);

                _context.Transactions.Remove(trans);
                await _context.SaveChangesAsync();

                await _budgetService.UpdateSpentAmount(trans.CategoryId, trans.CreatedAt.Month,
    trans.CreatedAt.Year);

                if (trans.EventId.HasValue)
                {
                    await _eventService.UpdateEventSpent((int)trans.EventId!);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
