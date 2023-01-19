using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IWalletService
    {
        public Task<IEnumerable<Wallet>> GetWallets(int userId);
        public Task<Wallet> CreateWallet(int userId, CreateWalletDto walletDto);
        public Task UpdateWallet(int walletId, int userid, UpdateWalletDto updateWallet);
        Task<bool> VerifyIsUserInWallet(int walletId, int userId);
        Task DeleteWallet(int walletId, int userId);
    }
}
