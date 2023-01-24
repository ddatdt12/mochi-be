using Microsoft.AspNetCore.Mvc;
using MochiApi.Models;
using Microsoft.AspNetCore.Authorization;
using MochiApi.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MochiApi.Dtos;
using MochiApi.DTOs;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        public DataContext _context { get; set; }
        public IMapper _mapper { get; set; }
        public AccountController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("invitations")]
        public async Task<IActionResult> GetInvitations([MinLength(4, ErrorMessage = "At lease 4 characters")] string email)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            var invitations = await _context.Invitations.Where(u => u.UserId == userId).ToListAsync();

            var invitationDtos = _mapper.Map<IEnumerable<InvitationDto>>(invitations);
            return Ok(new ApiResponse<IEnumerable<InvitationDto>>(invitationDtos, "Get invitations"));
        }
    }
}
