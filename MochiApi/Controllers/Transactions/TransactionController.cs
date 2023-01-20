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
    [Route("api/wallets/{walletId}/transactions")]
    [Protect]
    public class TransactionController : Controller
    {
        public ITransactionService _transactionService { get; set; }
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public TransactionController(ITransactionService transactionService, IMapper mapper, IWalletService walletService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _walletService = walletService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<TransactionDto>>))]
        public async Task<IActionResult> GetTransactions(int walletId, [FromQuery] TransactionFilterDto filter)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }
            var trans = await _transactionService.GetTransactions(walletId, filter);
            var transRes = _mapper.Map<IEnumerable<TransactionDto>>(trans);
            return Ok(new ApiResponse<IEnumerable<TransactionDto>>(transRes, "Get transaction successfully!"));
        }

        [HttpPost]
        [Produces(typeof(ApiResponse<TransactionDto>))]
        public async Task<IActionResult> CreateTransaction(int walletId, [FromBody] CreateTransactionDto createTransactionDto)
        {
            var user = HttpContext.Items["User"] as User;
            var userId = HttpContext.Items["UserId"] as int?;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }
            if (!await _categoryService.VerifyIsCategoryOfWallet(createTransactionDto.CategoryId, walletId))
            {
                throw new ApiException("Invalid category!", 400);
            }

            var trans = await _transactionService.CreateTransaction((int)userId ,walletId, createTransactionDto);

            var transRes = _mapper.Map<TransactionDto>(trans);
            return new CreatedResult("", new ApiResponse<TransactionDto>(transRes, "Create successfully"));
        }

        [HttpPut("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> UpdateTransaction(int id, int walletId, [FromBody] UpdateTransactionDto updateTransDto)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"] as int?);

            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }

            if (!await _categoryService.VerifyIsCategoryOfWallet((int)updateTransDto.CategoryId, walletId))
            {
                throw new ApiException("Invalid category!", 400);
            }

            await _transactionService.UpdateTransaction(id, walletId,updateTransDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> DeleteTrans(int id, int walletId)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId))
            {
                throw new ApiException("Access denied!", 400);
            }

            await _transactionService.DeleteTransaction(id);
            return NoContent();
        }
    }
}
