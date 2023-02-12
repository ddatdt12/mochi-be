using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Models;
using MochiApi.Services;
using System.ComponentModel.DataAnnotations;
using static MochiApi.Common.Enum;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/wallets/{walletId}/debts")]
    [Protect]
    public class DebtTransactionController : Controller
    {
        public ITransactionService _transactionService { get; set; }
        public DataContext _context { get; set; }
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public DebtTransactionController(ITransactionService transactionService, IMapper mapper, IWalletService walletService, ICategoryService categoryService, DataContext context)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _walletService = walletService;
            _categoryService = categoryService;
            _context = context;
        }

        [HttpGet]
        [Produces(typeof(List<DebtPerPerson>))]
        public async Task<IActionResult> GetDebts(int walletId, [FromQuery, Required] CategoryType type)
        {
            int userId = HttpContext.Items["UserId"] as int? ?? 0;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            if (type != CategoryType.Debt && type != CategoryType.Loan)
            {
                throw new ApiException("Only accept Debt or Loan!", 400);
            }
            var debts = await _context.Transactions.Where(t => t.CreatorId == userId && t.WalletId == walletId && t.Category!.Type == type).ToListAsync();
            var debtDtos = _mapper.Map<List<TransactionDto>>(debts);
            var res = debtDtos.GroupBy(d => d.UnknownParticipantsStr).Select(d => new DebtPerPerson { PersonName = d.Key, Details = d.ToList() }).ToList();
            var resDtos = _mapper.Map<List<DebtPerPerson>>(res);
            return Ok(new ApiResponse<List<DebtPerPerson>>(resDtos, "Get debts successfully!"));
        }

        [HttpGet("transactions")]
        [Produces(typeof(List<TransactionDto>))]
        public async Task<IActionResult> GetTransactionsDebt(int walletId, [FromQuery, Required] CategoryType type)
        {
            int userId = HttpContext.Items["UserId"] as int? ?? 0;
            if (!await _walletService.VerifyIsUserInWallet(walletId, (int)userId!))
            {
                throw new ApiException("Access denied!", 400);
            }
            if (type != CategoryType.Debt && type != CategoryType.Loan)
            {
                throw new ApiException("Only accept Debt or Loan!", 400);
            }
            var debts = await _context.Transactions.Where(t => t.CreatorId == userId && t.WalletId == walletId && t.Category!.Type == type && t.AccumulatedAmount != t.Amount).ToListAsync();

            var transes = _mapper.Map<List<TransactionDto>>(debts);
            return Ok(new ApiResponse<object>(transes, "Get debts successfully!"));
        }
    }

    public class DebtPerPerson
    {
        public DebtPerPerson()
        {
            Details = new List<TransactionDto>();
        }

        public string PersonName { get; set; }
        public List<TransactionDto> Details { get; set; }
    }
}
