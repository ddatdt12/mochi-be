using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransaction(int userId, int walletId, CreateTransactionDto transDto);
        Task<IEnumerable<Transaction>> GetTransactions(int walletId, TransactionFilterDto filter);
        Task DeleteTransaction(int transactionId);
        Task UpdateTransaction(int transactionId, int walletId, UpdateTransactionDto updateTransDto);
    }
}
