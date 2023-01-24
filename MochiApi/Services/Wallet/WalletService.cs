using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Hubs;
using MochiApi.Models;
using static MochiApi.Common.Enum;

namespace MochiApi.Services
{
    public class WalletService : IWalletService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        IHubContext<NotiHub> _notiHub;
        public DataContext _context { get; set; }

        public WalletService(IConfiguration configuration, DataContext context, IMapper mapper, IHubContext<NotiHub> notiHub)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _notiHub = notiHub;
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
                if (walletDto.Type == WalletType.Group && (walletDto.MemberIds.Count == 0))
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
                    Role = MemberRole.Admin,
                    WalletId = wallet.Id,
                });

                members.AddRange(walletDto.MemberIds.Select(mId =>
                new WalletMember
                {
                    WalletId = wallet.Id,
                    Role = MemberRole.Member,
                    UserId = mId,
                }));

                IEnumerable<Notification>? notis = null;
                //Invitations
                if (wallet.Type == WalletType.Group)
                {
                    var now = DateTime.UtcNow;
                    var invitations = walletDto.MemberIds.Select(m => new Invitation
                    {
                        SenderId = userId,
                        CreatedAt = now,
                        UserId = m,
                        Status = InvitationStatus.New,
                        ExpirationDate = now.AddDays(7),
                        WalletId = wallet.Id
                    });

                    await _context.Invitations.AddRangeAsync(invitations);
                    await _context.SaveChangesAsync();
                    notis = invitations.Select(i => new Notification
                    {
                        InvitationId = i.Id,
                        UserId = i.UserId,
                        Type = NotificationType.JoinWalletInvitation,
                        Description = Helper.NotiTemplate.GetInvitationContent(wallet.Name)
                    });
                    await _context.Notifcations.AddRangeAsync(notis);
                }
                await _context.WalletMembers.AddRangeAsync(members);
                await _context.Categories.AddRangeAsync(cates);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                if (notis != null)
                {
                    var notisDtos = _mapper.Map<IEnumerable<NotificationDto>>(notis);
                    foreach (var item in notisDtos)
                    {
                        _ = _notiHub.Clients.Users(item.UserId.ToString()).SendAsync("Notification", item);
                    }
                }
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
                throw new ApiException("Wallet not found!", 400);
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
