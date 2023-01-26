using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.Dtos.Auth;
using MochiApi.Dtos.User;
using MochiApi.DTOs;
using MochiApi.Models;
using MochiApi.Services;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public DataContext _context { get; set; }
        public IMapper _mapper { get; set; }

        public AuthController(IAuthService auth, DataContext context, IMapper mapper)
        {
            _auth = auth;
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Produces(typeof(ApiResponse<UserDto>))]
        [Protect]
        public IActionResult Auth()
        {
            var user = (User)(HttpContext.Items["User"] as User)!;
            //var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var userDTO = _mapper.Map<UserDto>(user);
            return Ok(new ApiResponse<object>(userDTO, "Authentication successfully"));
        }
        [HttpPost("login")]
        [Produces(typeof(ApiResponse<UserDto>))]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            (User user, string token) = await _auth.Login(loginUser);
            var userDTO = _mapper.Map<UserDto>(user);
            return Ok(new
            {
                data = userDTO,
                token = token
            });
        }

        [HttpPost("register")]
        [Produces(typeof(ApiResponse<string>))]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUser)
        {
            await _auth.Register(registerUser);
            return Ok(new ApiResponse<string>(String.Empty, "Send email verification successfully"));
        }

        [HttpPost("verify-account")]
        [Produces(typeof(ApiResponse<UserDto>))]
        public async Task<IActionResult> VerifyEmailToken([FromBody] TokenDTO tokenDTO)
        {
            (User user, string token) = await _auth.VerifyEmailToken(tokenDTO);
            var userDTO = _mapper.Map<UserDto>(user);
            return Ok(new ApiResponse<UserDto>(userDTO, "Verify account successfully!"));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailUserDto user)
        {
            await _auth.ForgotPassword(user.Email);
            return Ok(new ApiResponse<string>(string.Empty, "Please check the code in your email. This code consists of 4 numbers."));
        }

        [HttpPost("verify-code-repassword")]
        public IActionResult VerifyResetPassword([FromBody] TokenForgotPasswordDto user)
        {
            string token = _auth.VerifyResetPassword(user.Email, user.Code);
            return Ok(new ApiResponse<string>(token, "Verify code successfully!"));
        }

        [HttpPost("reset-password")]
        [Produces(typeof(ApiResponse<UserDto>))]
        public async Task<IActionResult> ResetPassword(TokenResetPasswordDto reUser)
        {
            (User user, string token) = await _auth.ResetPassword(reUser);
            var userDTO = _mapper.Map<UserDto>(user);
            return Ok(new ApiResponse<UserDto>(userDTO, "Reset Password successfully."));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword chUser)
        {
            await _auth.ChangePassword(chUser);
            return Ok(new ApiResponse<string>(string.Empty, "Change Password successfully."));
        }
    }
}