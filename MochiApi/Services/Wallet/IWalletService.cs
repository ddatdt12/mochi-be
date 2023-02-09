using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IWalletService
    {
        public Task<IEnumerable<Wallet>> GetWallets(int userId);
        public Task<Wallet> CreateWallet(int userId, CreateWalletDto walletDto);
        public Task UpdateWallet(int walletId, int userid, UpdateWalletDto updateWallet);
        public Task UpdateWalletBalance(int walletId, int amount);
        Task<bool> VerifyIsUserInWallet(int walletId, int userId);
        Task DeleteWallet(int walletId, int userId);
        Task<IEnumerable<WalletMember>> GetUsersInWallet(int walletId, int userId);
        Task DeleteMemberInWallet(int userId, int walletId, int memberId);
        Task AddMembersToWallet(int userId, int walletId, List<CreateWalletMemberDto> createDto);
        Task UpdateMembersToWallet(int userId, int walletId, List<CreateWalletMemberDto> updateDtos);
    }
}
