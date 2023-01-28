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
    [Route("api/global-wallets")]
    [Protect]
    public class GlobalWalletController : Controller
    {
        public ITransactionService _transactionService { get; set; }
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public GlobalWalletController(ITransactionService transactionService, IMapper mapper, IWalletService walletService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _walletService = walletService;
            _categoryService = categoryService;
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
    }
}
