using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.Dtos.Statistic;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Helper;
using MochiApi.Models;
using MochiApi.Services;
using static MochiApi.Common.Enum;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/global-wallets")]
    [Protect]
    public class GlobalWalletController : Controller
    {
        public ITransactionService _transactionService { get; set; }
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public DataContext _context { get; set; }
        public IMapper _mapper { get; set; }
        public GlobalWalletController(ITransactionService transactionService, IMapper mapper, IWalletService walletService, ICategoryService categoryService, DataContext context)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _walletService = walletService;
            _categoryService = categoryService;
            _context = context;
        }

        [HttpGet("transactions")]
        [Produces(typeof(IEnumerable<TransactionDto>))]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilterDto filter)
        {
            int userId = HttpContext.Items["UserId"] as int? ?? 0;
            var transList = await _transactionService.GetTransactions(userId, null, filter);
            var transRes = _mapper.Map<IEnumerable<TransactionDto>>(transList);
            var groupByDates = transRes.GroupBy(tr => tr.CreatedAt.Date);
            List<TransactionGroupDateDto> response = new List<TransactionGroupDateDto>();
            long totalIncome = 0, totalExpense = 0;
            foreach (var group in groupByDates)
            {
                TransactionGroupDateDto item = new TransactionGroupDateDto { Date = group.Key };
                long revenue = 0;

                foreach (var transaction in group)
                {
                    if (Utils.PlusCategoryTypes.Contains(transaction.Category!.Type))
                    {
                        totalIncome += transaction.Amount;
                        revenue += transaction.Amount;
                    }
                    else
                    {
                        totalExpense += transaction.Amount;
                        revenue -= transaction.Amount;
                    }
                    item.Transactions.Add(transaction);
                }

                item.Revenue = revenue;
                response.Add(item);
            }

            return Ok(new ApiResponse<object>(new
            {
                totalIncome,
                totalExpense,
                details = response
            }, "Get transaction statistic group by date successfully!"));
        }

        [HttpGet("transactions/recently")]
        [Produces(typeof(IEnumerable<TransactionDto>))]
        public async Task<IActionResult> GetRecentlyTransactions([FromQuery] TransactionFilterDto filter)
        {
            int userId = HttpContext.Items["UserId"] as int? ?? 0;
            filter.Skip = 0;
            filter.Take = 5;
            var transList = await _transactionService.GetTransactions(userId, null, filter);
            var transRes = _mapper.Map<IEnumerable<TransactionDto>>(transList);
            return Ok(new ApiResponse<object>(transRes, "Get recently transactions successfully!"));
        }

        [HttpGet("group")]
        public async Task<IActionResult> GetReportGroupByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;
            startDate = startDate.Date;
            endDate = endDate.Date;
            if (startDate >= endDate)
            {
                throw new ApiException("End date must be greater than start date", 400);
            }

            int duration = endDate.Subtract(startDate).Days + 1;

            if (duration > 35)
            {
                throw new ApiException("End date can not be greater than start time 35 days", 400);
            }
            List<DailyReport> listDailyReport = new List<DailyReport>();
            endDate = endDate.AddDays(1).AddSeconds(-1);
            var transQuery = _context.Transactions.AsNoTracking().Where(t => t.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == MemberStatus.Accepted)
            && t.CreatedAt >= startDate && t.CreatedAt <= endDate);

            var dailyReportMap = (await transQuery.Select(t => new {t.CreatedAt, t.Category, t.Amount}).GroupBy(t => t.CreatedAt.Date).Select(gr =>
            new DailyReport
            {
                Date = gr.Key,
                Expense = gr.Where(i => Utils.MinusCategoryTypes.Contains(i.Category!.Type)).Sum(t => t.Amount),
                Income = gr.Where(i => Utils.PlusCategoryTypes.Contains(i.Category!.Type)).Sum(t => t.Amount),
            }).ToListAsync()).ToDictionary(r => r.Date, r => r);

            for (int i = 0; i < duration; i++)
            {
                DateTime date = startDate.AddDays(i);
                var report = dailyReportMap.ContainsKey(date) ? dailyReportMap[date] : new DailyReport { Date = date, Income = 0, Expense = 0 };
                listDailyReport.Add(report);
            }

            long netIncome = dailyReportMap.Values.Sum(t => t.Income - t.Expense);

            return Ok(new ApiResponse<object>(new
            {
                dailyReports = listDailyReport,
                netIncome = netIncome,
            }, "Get report of wallet successfully!"));
        }
        [HttpGet("expense")]
        public async Task<IActionResult> GetSpendingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, bool showTopSpending = true)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;
            startDate = startDate.Date;
            endDate = endDate.Date;
            if (startDate >= endDate)
            {
                throw new ApiException("End date must be greater than start date", 400);
            }

            int duration = endDate.Subtract(startDate).Days + 1;

            if (duration > 35)
            {
                throw new ApiException("End date can not be greater than start time 35 days", 400);
            }

            endDate = endDate.AddDays(1).AddSeconds(-1);
            var transQuery = _context.Transactions.AsNoTracking().Where(t => t.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == MemberStatus.Accepted)
            && Utils.MinusCategoryTypes.Contains(t.Category!.Type)&& t.CreatedAt >= startDate && t.CreatedAt <= endDate);

            var listTopSpending = new List<CategoryStat>();
            if (showTopSpending)
            {
                listTopSpending = (await transQuery.GroupBy(t => t.CategoryId).Select(gr =>
                new
                {
                    CategoryId = gr.Key,
                    Amount = gr.Sum(t => t.Amount),
                    Category = gr.FirstOrDefault()!.Category!,
                }
                ).ToListAsync()).Select(i => new CategoryStat
                {
                    Category = new CategoryDto
                    {
                        Id = i.Category.Id,
                        Icon = i.Category.Icon,
                        Name = i.Category.Name,
                        Type = i.Category.Type,
                        Group = i.Category.Group
                    },
                    Amount = i.Amount
                }).ToList();
            }

            long expense = await transQuery.SumAsync(t => t.Amount);

            return Ok(new ApiResponse<object>(new
            {
                details = listTopSpending,
                totalAmount = expense,
            }, "statistic wallet successfully!"));
        }

        [HttpGet("income")]
        public async Task<IActionResult> GetIncomeReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, bool showTopIncome = true)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;
            startDate = startDate.Date;
            endDate = endDate.Date;
            if (startDate >= endDate)
            {
                throw new ApiException("End date must be greater than start date", 400);
            }

            int duration = endDate.Subtract(startDate).Days + 1;

            if (duration > 35)
            {
                throw new ApiException("End date can not be greater than start time 35 days", 400);
            }


            endDate = endDate.AddDays(1).AddSeconds(-1);
            var transQuery = _context.Transactions.AsNoTracking().Where(t => t.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == MemberStatus.Accepted)
            && Utils.PlusCategoryTypes.Contains(t.Category!.Type) && t.CreatedAt >= startDate && t.CreatedAt <= endDate);

            var incomes = new List<CategoryStat>();
            if (showTopIncome)
            {
                incomes = (await transQuery.GroupBy(t => t.CategoryId).Select(gr =>
                new
                {
                    CategoryId = gr.Key,
                    Amount = gr.Sum(t => t.Amount),
                    Category = gr.FirstOrDefault()!.Category!,
                }
                ).ToListAsync()).Select(i => new CategoryStat
                {
                    Category = new CategoryDto
                    {
                        Id = i.Category.Id,
                        Icon = i.Category.Icon,
                        Name = i.Category.Name,
                        Type = i.Category.Type,
                        Group = i.Category.Group
                    },
                    Amount = i.Amount
                }).ToList();
            }

            long expense = await transQuery.SumAsync(t => t.Amount);

            return Ok(new ApiResponse<object>(new
            {
                details = incomes,
                totalAmount = expense,
            }, "statistic wallet successfully!"));
        }
    }
}
