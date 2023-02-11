using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransaction(int userId, int walletId, CreateTransactionDto transDto);
        Task DeleteTransaction(int transactionId);
        Task UpdateTransaction(int transactionId, int walletId, UpdateTransactionDto updateTransDto);
        Task<IEnumerable<Transaction>> GetTransactions(int userId, int? walletId, TransactionFilterDto filter);
        Task<Transaction?> GetTransactionById(int id);
        Task<List<Transaction>> GetChildTransactionsOfParentTrans(int id);
    }
}
