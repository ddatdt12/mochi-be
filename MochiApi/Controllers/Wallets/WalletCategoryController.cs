using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MochiApi.Attributes;
using MochiApi.Dtos;
using MochiApi.DTOs;
using MochiApi.Error;
using MochiApi.Models;
using MochiApi.Services;
using Newtonsoft.Json;

namespace MochiApi.Controllers
{
    [ApiController]
    [Route("api/wallets/{walletId}/categories")]
    [Protect]
    public class WalletCategoryController : Controller
    {
        public ICategoryService _categoryService { get; set; }
        public IWalletService _walletService { get; set; }
        public IMapper _mapper { get; set; }
        public WalletCategoryController(ICategoryService category, IMapper mapper, IWalletService walletService)
        {
            _categoryService = category;
            _mapper = mapper;
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(int walletId)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            var cates = await _categoryService.GetCategories(walletId);
            var catesRes = _mapper.Map<IEnumerable<CategoryDto>>(cates);
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>(catesRes, "Get categories successfully!"));
        }

        [HttpPost]
        [Produces(typeof(ApiResponse<CategoryDto>))]
        public async Task<IActionResult> CreateCategory(int walletId,[FromBody] CreateCategoryDto createCategoryDto)
        {
            var user = HttpContext.Items["User"] as User;

            var cate = await _categoryService.CreateCategory(walletId, createCategoryDto);

            var cateDto = _mapper.Map<CategoryDto>(cate);
            return new CreatedResult("", new ApiResponse<CategoryDto>(cateDto, "Create successfully"));
        }

        [HttpPut("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> UpdateCategory(int id, int walletId, [FromBody] UpdateCategoryDto categoryDTO)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            await _categoryService.UpdateCategory(id, walletId, categoryDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> DeleteCategory(int id, int walletId)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            await _categoryService.DeleteCategory(id, walletId);
            return NoContent();
        }
    }
}
