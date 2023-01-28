using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Models;

namespace MochiApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _configuration;
        private readonly IBudgetService _budgetService;
        private readonly IWalletService _walletService;
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
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

        public async Task<IEnumerable<Transaction>> GetTransactions(int userId ,int? walletId, TransactionFilterDto filter)
        {
            var transQuery = _context.Transactions.AsQueryable();

            if (walletId.HasValue)
            {
                transQuery = transQuery.Where(t => t.WalletId == walletId);
            }
            else
            {
                transQuery = transQuery.Where(t => t.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted));
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

                int amount = transDto.Amount;
                var cate = await _context.Categories.Where(c => c.Id == trans.CategoryId).FirstOrDefaultAsync();
                await _budgetService.UpdateSpentAmount(trans.CategoryId, trans.CreatedAt.Month,
                    trans.CreatedAt.Year, trans.Amount, saveChanges: false);
                if (cate!.Type == Common.Enum.CategoryType.Expense)
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


                if (trans.EventId.HasValue && !await _eventService.CheckEventIsInWallet((int)trans.EventId, trans.WalletId, trans.CreatorId))
                {
                    throw new ApiException("Invalid Event", 400);
                }

                if (updateTransDto.CategoryId != trans.CategoryId)
                {
                    await _budgetService.UpdateSpentAmount(trans.CategoryId, updateTransDto.CreateAtValue.Month,
                    updateTransDto.CreateAtValue.Year, updateTransDto.Amount, saveChanges: false);

                    await _budgetService.UpdateSpentAmount(trans.CategoryId, trans.CreatedAt.Month,
                    trans.CreatedAt.Year, -1 * trans.Amount, saveChanges: false);

                    var updatedCate = await _context.Categories.Where(c => c.Id == updateTransDto.CategoryId).FirstOrDefaultAsync();
                    int amount = trans.Amount;
                    int updatedAmount = updateTransDto.Amount;
                    if (updatedCate!.Type == Common.Enum.CategoryType.Expense)
                    {
                        updatedAmount *= -1;
                    }
                    if (trans.Category!.Type == Common.Enum.CategoryType.Income)
                    {
                        amount *= -1;
                    }
                    await _walletService.UpdateWalletBalance(walletId, amount + updatedAmount);
                }
                else if (updateTransDto.Amount != trans.Amount)
                {
                    await _budgetService.UpdateSpentAmount(trans.CategoryId, updateTransDto.CreateAtValue.Month,
            updateTransDto.CreateAtValue.Year, updateTransDto.Amount - trans.Amount, saveChanges: false);

                    int amount = updateTransDto.Amount - trans.Amount;
                    if (trans.Category!.Type == Common.Enum.CategoryType.Expense)
                    {
                        amount *= -1;
                    }
                    await _walletService.UpdateWalletBalance(walletId, amount);
                }
                _mapper.Map(updateTransDto, trans);

                await _context.SaveChangesAsync();
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

                await _budgetService.UpdateSpentAmount(trans.CategoryId, trans.CreatedAt.Month,
                    trans.CreatedAt.Year, -1 * trans.Amount, saveChanges: false);

                int amount = trans.Amount;
                if (trans.Category!.Type == Common.Enum.CategoryType.Income)
                {
                    amount *= -1;
                }
                await _walletService.UpdateWalletBalance(trans.WalletId, amount);

                _context.Transactions.Remove(trans);
                await _context.SaveChangesAsync();

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
