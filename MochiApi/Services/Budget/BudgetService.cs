﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Models;
using System.Linq.Expressions;

namespace MochiApi.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public BudgetService(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Budget>> GetBudgets(int walletId, int month, int year)
        {
            var budgets = await _context.Budgets.AsNoTracking().Where(b => b.WalletId == walletId && b.Month == month && b.Year == year)
            .Include(b => b.Category)
            .ToListAsync();

            return budgets;
        }
        public async Task<BudgetSummary> SummaryBudget(int walletId, int month, int year)
        {
            var budgetsQuery = _context.Budgets.AsNoTracking().Where(b => b.WalletId == walletId && b.Month == month && b.Year == year);

            var budgetSum = new BudgetSummary
            {
                TotalBudget = await budgetsQuery.SumAsync(b => b.LimitAmount),
                TotalSpentAmount = await budgetsQuery.SumAsync(b => b.SpentAmount),
            };
            return budgetSum;
        }

        async Task<bool> IsBudgetExist(int categoryId, int month, int year, Expression<Func<Budget, bool>>? predicate = null)
        {
            var budgetQuery = _context.Budgets.Where(b => b.CategoryId == categoryId && b.Month == month && b.Year == year);
            if (predicate != null)
            {
                budgetQuery = budgetQuery.Where(predicate);
            }
            return await budgetQuery.AnyAsync();
        }

        public async Task<Budget> CreateBudget(int userId, CreateBudgetDto createDto)
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
                budget.SpentAmount += amount;
                if (saveChanges)
                {
                    await _context.SaveChangesAsync();
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
            var realDailyExpense = budget.SpentAmount / DateTime.UtcNow.Day;
            var summary = new BudgetDetailSummary
            {
                ExpectedExpense = realDailyExpense * remainDaysOfMonth + budget.SpentAmount,
                RealDailyExpense = realDailyExpense,
                TotalBudget = budget.LimitAmount,
                TotalSpentAmount = budget.SpentAmount,
                RecommendedDailyExpense = budget.RemainingAmount / realDailyExpense,
            };

            return summary;
        }

        async Task<Budget?> IBudgetService.GetBudget(int id, int walletId, int month, int year)
        {
            var budget = await _context.Budgets.AsNoTracking().Where(b => b.Id == id && b.WalletId == walletId && b.Month == month && b.Year == year)
                 .FirstOrDefaultAsync();

            return budget;
        }

        async Task<IEnumerable<BudgetDetailStatistic>> IBudgetService.StatisticBudget(int id, int walletId, int month, int year)
        {
            var budget = await _context.Budgets.Where(b => b.Id == id && b.WalletId == walletId && b.Month == month && b.Year == year)
             .FirstOrDefaultAsync();

            if (budget == null)
            {
                throw new ApiException("Invalid budget", 400);
            }

            int day = DateTime.DaysInMonth(year, month);

            //if (year == DateTime.Now.Year && month == DateTime.Now.Month)
            //{
            //    day = DateTime.Now.Day;
            //}

            List<long> amountEachDay = new List<long>(new long[day]);

            DateTime startDate = new DateTime(year, month, 1).ToUniversalTime();
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1).ToUniversalTime();
            var statistic = await _context.Transactions.Where(t => t.CategoryId == budget.CategoryId && t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .GroupBy(t => t.CreatedAt.Date)
            .Select(gr => new { Date = gr.Key, ExpenseAmount = gr.Sum(t => t.Amount) }).ToListAsync();

            foreach (var item in statistic)
            {
                amountEachDay[item.Date.Day - 1] += item.ExpenseAmount;
            }


            return amountEachDay.Select((v, i) => new BudgetDetailStatistic { Date = new DateTime(year, month, i + 1), ExpenseAmount = v }).ToList();
        }
    }
}