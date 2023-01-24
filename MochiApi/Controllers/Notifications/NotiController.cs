using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    [Protect]
    [Route("api/notifications")]
    public class NotiController : ControllerBase
    {
        private readonly INotiService _notiService;
        public IMapper _mapper { get; set; }

        public NotiController(IMapper mapper, INotiService notiService)
        {
            _notiService = notiService;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<NotificationDto>>))]
        public async Task<IActionResult> GetNotis()
        {
            var userId = HttpContext.Items["UserId"] as int?;
            var notis = await _notiService.GetNotifications((int)userId!);
            var notiDtos = _mapper.Map<IEnumerable<NotificationDto>>(notis);
            return Ok(new ApiResponse<object>(notiDtos, "Get notifications"));
        }
    }
}