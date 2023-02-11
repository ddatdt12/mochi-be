using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IBudgetService
    {
        Task<Budget> CreateBudget(int userId, CreateBudgetDto createDto);
        Task DeleteBudget(int id, int walletId);
        Task<IEnumerable<Budget>> GetBudgets(int walletId, int month, int year);
        Task<Budget> GetBudgetById(int id, int walletId, int month, int year);
        Task<BudgetSummary> SummaryBudget(int walletId, int month, int year);
        Task UpdateBudget(int id, int walletId, UpdateBudgetDto updateDto);
        Task<BudgetDetailSummary> SummaryBudgetDetail(int id, int walletId, int month, int year);
        Task<IEnumerable<BudgetDetailStatistic>> StatisticBudget(int id, int walletId, int month, int year);
        Task UpdateSpentAmount(int categoryId, int month, int year);
    }
}
