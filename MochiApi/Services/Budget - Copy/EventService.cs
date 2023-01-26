using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Helper;
using MochiApi.Hubs;
using MochiApi.Models;
using System.Linq.Expressions;

namespace MochiApi.Services
{
    public class EventService : IEventService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public DataContext _context { get; set; }

        public EventService(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Event>> GetEvents(int userId)
        {
            var @events = await _context.Events.Include(e => e.Wallet).Where(e =>
          (e.WalletId != null && e.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted))
          || (e.WalletId == null && e.CreatorId == userId)
          ).ToListAsync();

            return @events;
        }
        public async Task<IEnumerable<Event>> GetEventsOfWallet(int userId, int walletId)
        {
            var @events = await _context.Events.AsNoTracking().Include(e => e.Wallet).Where(e =>
          (e.WalletId == walletId && e.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted))
          || (e.WalletId == null && e.CreatorId == userId)
          ).ToListAsync();

            return @events;
        }
        public async Task<Event?> GetEventById(int id, int userId)
        {
            var @event = await _context.Events.AsNoTracking()
            .Where(e => e.Id == id && (e.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted))
          || (e.WalletId == null && e.CreatorId == userId)
          ).Include(e => e.Wallet).FirstOrDefaultAsync();

            return @event;
        }
        public async Task<IEnumerable<Transaction>> GetTransactionOfEvent(int id)
        {
            var trans = await _context.Transactions.AsNoTracking().Where(b => b.EventId != id).OrderByDescending(tr => tr.CreatedAt)
            .ToListAsync();

            return trans;
        }


        public async Task<Budget> CreateEvent()
        {
            var cate = await _context.Categories.Where(c => c.Id == createDto.CategoryId).FirstOrDefaultAsync();

            if (cate == null || cate.Type == Common.Enum.CategoryType.Income)
            {
                throw new ApiException("Invalid category", 400);
            }

            if (await IsBudgetExist(createDto.CategoryId, createDto.Month, createDto.Year))
            {
                throw new ApiException("Budget for this category in this time duration have already existed!", 400);
            }

            var budget = _mapper.Map<Budget>(createDto);

            var spentInMonth = _context.Transactions.Where(t => t.CategoryId == budget.CategoryId
            && t.CreatedAt.Month == createDto.Month && t.CreatedAt.Year == createDto.Year).Sum(t => t.Amount);

            budget.CreatorId = userId;
            budget.SpentAmount = spentInMonth;
            await _context.Budgets.AddAsync(budget);

            await _context.SaveChangesAsync();
            return budget;
        }

        public async Task UpdateBudget(int id, int walletId, UpdateBudgetDto updateDto)
        {
            var budget = await _context.Budgets.Where(t => t.Id == id && t.WalletId == walletId).FirstOrDefaultAsync();
            if (budget == null)
            {
                throw new ApiException("Budget not found!", 400);
            }

            if (updateDto.CategoryId != budget.CategoryId)
            {
                var cate = await _context.Categories.Where(c => c.Id == updateDto.CategoryId).FirstOrDefaultAsync();

                if (cate == null || cate.Type == Common.Enum.CategoryType.Income)
                {
                    throw new ApiException("Invalid category", 400);
                }

                if (await IsBudgetExist(updateDto.CategoryId, updateDto.Month, updateDto.Year))
                {
                    throw new ApiException("Budget for this category in this time duration have already existed!", 400);
                }

                var spentInMonth = _context.Transactions.Where(t => t.CategoryId == updateDto.CategoryId
                && t.CreatedAt.Month == updateDto.Month && t.CreatedAt.Year == updateDto.Year).Sum(t => t.Amount);
                budget.SpentAmount = spentInMonth;
            }


            _mapper.Map(updateDto, budget);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSpentAmount(int categoryId, int month, int year, int amount, bool saveChanges = false)
        {
            var budget = await _context.Budgets.Where(b => b.CategoryId == categoryId && b.Month == month && b.Year == year).FirstOrDefaultAsync();

            if (budget != null)
            {
                long beforeSpendAmout = budget.SpentAmount;
                budget.SpentAmount += amount;

                IEnumerable<Notification>? notisList = null;
                if (beforeSpendAmout <= budget.LimitAmount && budget.SpentAmount > budget.LimitAmount)
                {
                    var memberIds = await _context.WalletMembers
                    .Where(wM => wM.WalletId == budget.WalletId && wM.Status == Common.Enum.MemberStatus.Accepted)
                    .Select(wM => wM.UserId).ToArrayAsync();

                    await _context.Entry(budget).Reference(b => b.Category).LoadAsync();

                    var notisDto = memberIds.Select(id => new CreateNotificationDto
                    {
                        UserId = id,
                        BudgetId = budget.Id,
                        WalletId = budget.WalletId,
                        Type = Common.Enum.NotificationType.BudgetExceed,
                        Description = NotiTemplate.GetRemindBudgetExceedLimit(budget.Category?.Name ?? "", month, year),
                    });

                    notisList = await _notiService.CreateListNoti(notisDto, false);
                }

                if (saveChanges)
                {
                    await _context.SaveChangesAsync();
                }

                if (notisList != null)
                {
                    var notisDtos = _mapper.Map<IEnumerable<NotificationDto>>(notisList);
                    foreach (var noti in notisList)
                    {
                        _ = _notiHub.Clients.User(noti.UserId.ToString()).SendAsync("Notification", noti);
                    }
                }
            }
        }
        public async Task DeleteBudget(int id, int walletId)
        {
            var budget = await _context.Budgets.Where(t => t.Id == id && t.WalletId == walletId).FirstOrDefaultAsync();
            if (budget == null)
            {
                throw new ApiException("Transaction not found!", 400);
            }
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }

        public async Task<BudgetDetailSummary> SummaryBudgetDetail(int id, int walletId, int month, int year)
        {
            var budget = await _context.Budgets.Where(b => b.Id == id && b.WalletId == walletId && b.Month == month && b.Year == year)
             .FirstOrDefaultAsync();

            if (budget == null)
            {
                throw new ApiException("Invalid budget", 400);
            }

            DateTime now = DateTime.Now;
            DateTime date = new DateTime(year, month, 1);
            if (now.Year != year || now.Month != month)
            {
                var RealDailyExpense = budget.SpentAmount / DateTime.DaysInMonth(year, month);
                return new BudgetDetailSummary
                {
                    TotalBudget = budget.LimitAmount,
                    TotalSpentAmount = budget.SpentAmount,
                    ExpectedExpense = 0,
                    RealDailyExpense = RealDailyExpense
                };
            }


            int remainDaysOfMonth = DateTime.DaysInMonth(year, month) - DateTime.UtcNow.Day;
            double realDailyExpense = budget.SpentAmount * 1.0 / DateTime.UtcNow.Day;
            var summary = new BudgetDetailSummary
            {
                ExpectedExpense = realDailyExpense * remainDaysOfMonth + budget.SpentAmount,
                RealDailyExpense = realDailyExpense,
                TotalBudget = budget.LimitAmount,
                TotalSpentAmount = budget.SpentAmount,
                RecommendedDailyExpense = remainDaysOfMonth == 0 ? 0 : budget.RemainingAmount / remainDaysOfMonth,
            };

            return summary;
        }
    }
}
