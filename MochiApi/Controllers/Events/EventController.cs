using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Models;
using MochiApi.Services;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/events")]
    [Protect]
    public class EventController : Controller
    {
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public DataContext _context { get; set; }
        public EventController(IMapper mapper, IWalletService walletService, DataContext context)
        {
            _mapper = mapper;
            _walletService = walletService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var userId = HttpContext.Items["UserId"] as int?;

          
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return Ok(new ApiResponse<object>(eventDtos, "Get my events successfully!"));
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

            var trans = await _transactionService.CreateTransaction((int)userId, walletId, createTransactionDto);

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

            await _transactionService.UpdateTransaction(id, walletId, updateTransDto);
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
