using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{

    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> GetCategories(int walletId);
        public Task<IEnumerable<Category>> GetCategories();
        public Task<Category> CreateCategory(int walletId, CreateCategoryDto categoryDto);
        public Task UpdateCategory(int id, int walletId, UpdateCategoryDto updateCate);
        Task<bool> DeleteCategory(int walletId, int cateId);
        Task<bool> VerifyIsCategoryOfWallet(int categoryId, int walletId);
        //public string SaveIcon(ImageUpload imageUpload);
    }
}
