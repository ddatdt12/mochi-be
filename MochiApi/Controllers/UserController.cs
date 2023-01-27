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
    [Route("api/users")]
    public class UserController : Controller
    {
        public DataContext _context { get; set; }
        public IMapper _mapper { get; set; }
        public UserController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetUsers([EmailAddress] string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            var userDto = _mapper.Map<BasicUserDto>(user);
            return Ok(new ApiResponse<BasicUserDto>(userDto, "search users by email"));
        }
    }
}
