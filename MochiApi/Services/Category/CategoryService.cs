﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MochiApi.Dtos;
using MochiApi.Dtos.Auth;
using MochiApi.Error;
using MochiApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MochiApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public CategoryService(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.Where(c => c.WalletId == null).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories(int walletId)
        {
            return await _context.Categories.Where(c => c.WalletId == walletId).ToListAsync();
        }

        public async Task<Category> CreateCategory(int walletId, CreateCategoryDto categoryDto)
        {
            var nameExist = await _context.Categories.Where(c => c.WalletId == categoryDto.WalletId && c.Name == categoryDto.Name).AnyAsync();
            if (nameExist)
            {
                throw new ApiException("Category already exists", 400);
            }
            if (categoryDto.Type == Common.Enum.CategoryType.Income)
            {
                categoryDto.Group = Common.Enum.CategoryGroup.Income;
            }
            else
            {
                if (categoryDto.Group == Common.Enum.CategoryGroup.Income)
                {
                    throw new ApiException("Invalid Category", 400);
                }
            }
            categoryDto.WalletId = walletId;
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategory(int id, int walletId, UpdateCategoryDto updateCate)
        {
            var category = await _context.Categories.Where(p => p.Id == id && p.WalletId == walletId).FirstOrDefaultAsync();
            if (category == null)
            {
                throw new ApiException("Category not found", 404);
            }

            if (updateCate.Type.HasValue)
            {
                if (updateCate.Type == Common.Enum.CategoryType.Income)
                {
                    updateCate.Group = Common.Enum.CategoryGroup.Income;
                }
                else
                {
                    if (updateCate.Group == Common.Enum.CategoryGroup.Income)
                    {
                        throw new ApiException("Invalid Category", 400);
                    }
                }
            }

            var nameExist = await _context.Categories.Where(c => c.Id != id && c.WalletId == walletId && c.Name == updateCate.Name).AnyAsync();
            if (nameExist)
            {
                throw new ApiException("Category already exists", 400);
            }

            _mapper.Map(updateCate, category);
            await _context.SaveChangesAsync();
        }

        Task<bool> ICategoryService.DeleteCategory(int walletId, int cateId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyIsCategoryOfWallet(int categoryId, int walletId)
        {
            return await _context.Categories.Where(c => c.Id == categoryId && c.WalletId == walletId).AnyAsync();
        }
    }
}
