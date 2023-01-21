using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Models;
using MochiApi.Services;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/wallets/{walletId}/budgets")]
    [Protect]
    public class BudgetController : Controller
    {
        public IBudgetService _budgetService { get; set; }
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public BudgetController(IBudgetService budgetService, IMapper mapper, IWalletService walletService, ICategoryService categoryService)
        {
            _budgetService = budgetService;
            _mapper = mapper;
            _walletService = walletService;
            _categoryService = categoryService;
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

        [HttpGet("summary")]
        [Produces(typeof(ApiResponse<BudgetSummary>))]
        public async Task<IActionResult> SummaryBudget(int walletId, [FromQuery] int month, [FromQuery] int year)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            var budgetSummary = await _budgetService.StatisticBudget(walletId, month, year);

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
