using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Hubs;
using MochiApi.Models;
using MochiApi.Services;
using Newtonsoft.Json;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    [Protect]
    public class WalletController : Controller
    {
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }

        public WalletController(IWalletService walletSer, IMapper mapper)
        {
            _walletService = walletSer;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallets()
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;
            var wallets = await _walletService.GetWallets(userId);
            var walletsRes = _mapper.Map<IEnumerable<WalletDto>>(wallets);
            return Ok(new ApiResponse<IEnumerable<WalletDto>>(walletsRes, "Get my wallets successfully!"));


        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletDto createWalletDto)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;
            var wallet = await _walletService.CreateWallet(userId, createWalletDto);
            var walletDto = _mapper.Map<WalletDto>(wallet);

            return Ok(new ApiResponse<WalletDto>(walletDto, "Create wallet successfully!"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] UpdateWalletDto updateWallet)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;

            await _walletService.UpdateWallet(id, userId, updateWallet);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;

            await _walletService.DeleteWallet(id, userId);

            return NoContent();
        }

        [HttpGet("{id}/members")]
        [Produces(typeof(ApiResponse<WalletMemberDto>))]
        public async Task<IActionResult> GetMembersOfWallet(int id)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;

            var members = await _walletService.GetUsersInWallet(id, userId);
            return Ok(new ApiResponse<object>(members, "Get Members of wallet"));
        }

        [HttpPost("{id}/members/add")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> GetMembersOfWallet(int id, [FromBody] int memberId)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;

            await _walletService.AddMemberToWallet(userId, id, memberId);
            return NoContent();
        }

        [HttpPost("{id}/members/remove")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> AddMember(int id, [FromBody] int memberId)
        {
            var userId = (int)(HttpContext.Items["UserId"] as int?)!;

            await _walletService.RemoveMemberFromWallet(userId, id, memberId);
            return NoContent();
        }

    }
}
