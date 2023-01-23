﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Models;
using MochiApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/wallets/{walletId}/budgets")]
    [Protect]
    public class BudgetController : Controller
    {
        public IBudgetService _budgetService { get; set; }
        public ITransactionService _transService { get; set; }
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public BudgetController(IBudgetService budgetService, IMapper mapper, IWalletService walletService, ICategoryService categoryService, ITransactionService transService)
        {
            _budgetService = budgetService;
            _mapper = mapper;
            _walletService = walletService;
            _categoryService = categoryService;
            _transService = transService;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<BudgetDto>>))]
        public async Task<IActionResult> GetBudgets(int walletId, [FromQuery] int month, [FromQuery] int year)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }
            var budgets = await _budgetService.GetBudgets(walletId, month, year);

            var budgetDtos = _mapper.Map<IEnumerable<BudgetDto>>(budgets);
            return Ok(new ApiResponse<object>(budgetDtos, "Get budgets successfully!"));
        }
        [HttpGet("{id}/transactions")]
        [Produces(typeof(ApiResponse<IEnumerable<BudgetDto>>))]
        public async Task<IActionResult> GetTransactionsInBudget(int id, int walletId, [FromQuery, Required] int month, [FromQuery, Required] int year)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            var budget = await _budgetService.GetBudget(id, walletId, month, year);
            if (budget == null)
            {
                throw new ApiException("Not found", 404);
            }
            var transList = await _transService.GetTransactions(walletId,
                filter: new TransactionFilterDto
                {
                    StartDate = new DateTime(year, month, 1),
                    EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                    CategoryId = budget.CategoryId
                }
            );

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
                    if (transaction.Category!.Type == Common.Enum.CategoryType.Income)
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
            }, "Get budgets successfully!"));
        }

        [HttpGet("{id}/summary")]
        [Produces(typeof(ApiResponse<BudgetDto>))]
        public async Task<IActionResult> SummaryBudgetDetail(int id, int walletId, [FromQuery] int month, [FromQuery] int year)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            var budget = await _budgetService.SummaryBudgetDetail(id, walletId, month, year);

            return Ok(new ApiResponse<object>(budget, "Summary budget in detail successfully!!"));
        }
        
        [HttpGet("{id}/statistic")]
        [Produces(typeof(ApiResponse<BudgetDto>))]
        public async Task<IActionResult> StatisticBudgetDetail(int id, int walletId, [FromQuery] int month, [FromQuery] int year)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            var statisticByDate = await _budgetService.StatisticBudget(id, walletId, month, year);

            return Ok(new ApiResponse<object>(statisticByDate, "Summary budget in detail successfully!!"));
        }

        [HttpGet("summary")]
        [Produces(typeof(ApiResponse<BudgetSummary>))]
        public async Task<IActionResult> SummaryBudget(int walletId, [FromQuery] int month, [FromQuery] int year)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            var budgetSummary = await _budgetService.SummaryBudget(walletId, month, year);

            return Ok(new ApiResponse<object>(budgetSummary, "Summary budget successfully!"));
        }

        [HttpPost]
        [Produces(typeof(ApiResponse<BudgetDto>))]
        public async Task<IActionResult> CreateBudget(int walletId, [FromBody] CreateBudgetDto createBudgetDto)
        {
            var user = HttpContext.Items["User"] as User;
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }
            if (!await _categoryService.VerifyIsCategoryOfWallet(createBudgetDto.CategoryId, walletId))
            {
                throw new ApiException("Invalid category!", 400);
            }

            createBudgetDto.WalletId = walletId;
            var budget = await _budgetService.CreateBudget((int)userId, createBudgetDto);

            var budgetDto = _mapper.Map<BudgetDto>(budget);
            return new CreatedResult("", new ApiResponse<BudgetDto>(budgetDto, "Create successfully"));
        }

        [HttpPut("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> UpdateBudget(int id, int walletId, [FromBody] UpdateBudgetDto updateBudgetDto)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"] as int?);

            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }

            if (!await _categoryService.VerifyIsCategoryOfWallet((int)updateBudgetDto.CategoryId, walletId))
            {
                throw new ApiException("Invalid category!", 400);
            }

            await _budgetService.UpdateBudget(id, walletId, updateBudgetDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> DeleteTrans(int id, int walletId)
        {
            var userId = HttpContext.Items["UserId"] as int?;

            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }

            await _budgetService.DeleteBudget(id, walletId);
            return NoContent();
        }
    }
}