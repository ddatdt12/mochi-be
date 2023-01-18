using Microsoft.AspNetCore.Mvc;
using MochiApi.Models;
using Microsoft.AspNetCore.Authorization;
using MochiApi.Attributes;
using Microsoft.EntityFrameworkCore;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        public DataContext _context { get; set; }
        public UserController(DataContext context)
        {
            _context = context;
        }

        [Protect]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new
            {
                data = await _context.Users.OrderBy(u => u.Id).ToListAsync()
            });
        }
    }
}
