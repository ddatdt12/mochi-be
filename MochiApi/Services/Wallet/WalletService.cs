using AutoMapper;
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
    public class WalletService : IWalletService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public WalletService(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Wallet>> GetWallets(int userId)
        {
            return await _context.Wallets.AsNoTracking().Include(w => w.Members).Where(w => w.Members.Where(m => m.Id == userId).Count() > 0).ToListAsync();
        }

        public async Task<Wallet> CreateWallet(int userId, CreateWalletDto walletDto)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                walletDto.MemberIds = walletDto.MemberIds.Where(mId => mId != userId).ToList();
                if (walletDto.Type == Common.Enum.WalletType.Group && (walletDto.MemberIds.Count == 0))
                {
                    throw new ApiException("Ví nhóm có ít nhất 1 người", 400);
                }

                var wallet = _mapper.Map<Wallet>(walletDto);
                await _context.Wallets.AddAsync(wallet);

                await _context.SaveChangesAsync();

                var cates = await _context.Categories.Where(c => c.WalletId == walletDto.ClonedCategoryWalletId).ToListAsync();

                cates.ForEach(c =>
                {
                    c.Id = 0;
                    c.WalletId = wallet.Id;
                });

                var members = new List<WalletMember>();
                members.Add(new WalletMember
                {
                    UserId = userId,
                    Role = Common.Enum.MemberRole.Admin,
                    WalletId = wallet.Id,
                });

                members.AddRange(walletDto.MemberIds.Select(mId =>
                new WalletMember
                {
                    WalletId = wallet.Id,
                    Role = Common.Enum.MemberRole.Member,
                    UserId = mId,
                }));

                await _context.WalletMembers.AddRangeAsync(members);
                await _context.Categories.AddRangeAsync(cates);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return wallet;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateWallet(int walletId, int userId, UpdateWalletDto updateWallet)
        {
            var wallet = await _context.Wallets.Where(w => w.Id == walletId && w.Members.Any(m => m.Id == userId)).FirstOrDefaultAsync();
            if (wallet == null)
            {
                throw new ApiException("Wallet not found!",400);
            }
            _mapper.Map(updateWallet, wallet);

            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteWallet(int walletId, int userId)
        {
            var wallet = await _context.Wallets.Where(w => w.Id == walletId && w.Members.Any(m => m.Id == userId)).FirstOrDefaultAsync();

            if (wallet == null)
            {
                throw new ApiException("Wallet not found!", 400);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyIsUserInWallet(int walletId, int userId)
        {
            var exist = await _context.Wallets.Where(w => w.Id == walletId && w.Members.Any(m => m.Id == userId)).AnyAsync();

            return exist;
        }

    }
}
