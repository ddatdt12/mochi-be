﻿using AutoMapper;
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
            return await _context.Wallets.AsNoTracking()
            .Include(w => w.WalletMembers)
            .ThenInclude(wM => wM.User)
            .Where(w =>
            w.WalletMembers.Where(m => m.UserId == userId && m.Status == MemberStatus.Accepted).Count() > 0)
            .ToListAsync();
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

                var cates = await _context.Categories.AsNoTracking().Where(c => c.WalletId == walletDto.ClonedCategoryWalletId).ToListAsync();

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
                    Status = MemberStatus.Accepted
                });

                members.AddRange(walletDto.MemberIds.Select(mId =>
                new WalletMember
                {
                    WalletId = wallet.Id,
                    Role = MemberRole.Member,
                    UserId = mId,
                }).ToList());
                await _context.WalletMembers.AddRangeAsync(members);
                await _context.Categories.AddRangeAsync(cates);

                IEnumerable<Notification>? notis = null;
                //Invitations
                if (wallet.Type == WalletType.Group)
                {
                    notis = await CreateInvitations(userId, wallet, walletDto.MemberIds.ToList());
                }

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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var wallet = await _context.Wallets.Include(w => w.WalletMembers).Where(w => w.Id == walletId && w.Members.Any(m => m.Id == userId)).FirstOrDefaultAsync();
                IEnumerable<Notification>? notis = null;

                if (wallet == null)
                {
                    throw new ApiException("Wallet not found!", 400);
                }
                var adminMember = wallet.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == userId).FirstOrDefault();
                if (adminMember == null || adminMember.Role != MemberRole.Admin)
                {
                    throw new ApiException("You are not authorized!", 401);
                }

                if (updateWallet.Type.HasValue)
                {
                    if (updateWallet.Type == WalletType.Group && wallet.Type == WalletType.Personal)
                    {
                        var memberIds = updateWallet?.MemberIds?.Where(mId => mId != userId).ToList();
                        if (memberIds == null || memberIds.Count == 0)
                        {
                            throw new ApiException("Ví nhóm có ít nhất 1 người", 400);
                        }
                        var alreadyMemberIds = await _context.WalletMembers.Where(m => m.WalletId == walletId && m.UserId != userId)
                        .Select(m => m.UserId).ToListAsync();

                        await _context.WalletMembers.AddRangeAsync(memberIds.Select(mId =>
                        new WalletMember
                        {
                            WalletId = wallet.Id,
                            Role = MemberRole.Member,
                            UserId = mId,
                        }).ToList());
                        notis = await CreateInvitations(userId, wallet, memberIds);
                    }
                    else
                    {
                        if (wallet.Type == WalletType.Group)
                        {
                            await _context.WalletMembers.Where(wM => wM.WalletId == wallet.Id && wM.Role == MemberRole.Member).DeleteFromQueryAsync();
                        }
                    }

                }

                _mapper.Map(updateWallet, wallet);

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
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteWallet(int walletId, int userId)
        {
            var wallet = await _context.Wallets.Include(w => w.WalletMembers)
            .Where(w => w.Id == walletId &&
            w.WalletMembers.Any(m => m.UserId == userId && m.Status == MemberStatus.Accepted)).FirstOrDefaultAsync();

            if (wallet == null)
            {
                throw new ApiException("Wallet not found!", 400);
            }

            var walletMember = wallet.WalletMembers.FirstOrDefault(w => w.UserId == userId);
            if (walletMember == null)
            {
                throw new ApiException("Wallet not found!", 400);
            }
            if (wallet.IsDefault)
            {
                throw new ApiException("Default wallet can not be deleted!", 400);
            }
            if (walletMember.Status != MemberStatus.Accepted || walletMember.Role != MemberRole.Admin)
            {
                throw new ApiException("You are not authorized to do this action!", 400);
            }

            _context.Remove(wallet);
            await _context.SaveChangesAsync();
        }
        public async Task AddMemberToWallet(int userId, int walletId, CreateWalletMemberDto createDto)
        {
            if (userId == createDto.UserId)
            {
                throw new ApiException("You can not add yourself to wallet", 400);
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var wallet = await _context.Wallets.Where(w => w.Id == walletId).Include(w => w.WalletMembers).FirstOrDefaultAsync();

                if (wallet == null)
                {
                    throw new ApiException("Wallet not found!", 400);
                }

                if (wallet.Type != WalletType.Group)
                {
                    throw new ApiException("Wallet is not group!", 400);
                }

                var owner = await _context.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == userId).FirstOrDefaultAsync();

                if (owner!.Role != MemberRole.Admin)
                {
                    throw new ApiException("You are not authorized to delete user", 400);
                }

                var member = await _context.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == createDto.UserId).FirstOrDefaultAsync();
                if (member != null)
                {
                    if (member.Status == MemberStatus.Declined)
                    {
                        throw new ApiException("The user has  declined the invitation to join the wallet .", 400);
                    }
                    else
                    if (member.Status == MemberStatus.Pending)
                    {
                        throw new ApiException("Invitation to the group is waiting to be accepted.", 400);
                    }
                    else
                    {
                        throw new ApiException("User is already a member.", 400);
                    }
                }

                var walletMember = new WalletMember
                {
                    WalletId = wallet.Id,
                    UserId = createDto.UserId,
                    Role = createDto.Role,
                    Status = MemberStatus.Pending
                };
                await _context.WalletMembers.AddAsync(walletMember);

                var notis = await CreateInvitations(userId, wallet, memberIds: new List<int>() { createDto.UserId });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if (notis != null)
                {
                    SendNotifications(notis);
                }

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateMemberToWallet(int userId, int walletId, CreateWalletMemberDto createDto)
        {
            if (userId == createDto.UserId)
            {
                throw new ApiException("You can not update yourself to wallet", 400);
            }

            var owner = await _context.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == userId).FirstOrDefaultAsync();

            if (owner == null || owner!.Role != MemberRole.Admin)
            {
                throw new ApiException("You are not authorized to update member", 400);
            }

            var member = await _context.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == createDto.UserId).FirstOrDefaultAsync();
            if (member == null)
            {
                throw new ApiException("Member not found", 404);
            }

            if (member != null)
            {
                if (member.Status == MemberStatus.Declined)
                {
                    throw new ApiException("The user has  declined the invitation to join the wallet .", 400);
                }
                else
                if (member.Status == MemberStatus.Pending)
                {
                    throw new ApiException("Invitation to the group is waiting to be accepted.", 400);
                }
            }

            member!.Role = createDto.Role;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMemberInWallet(int userId, int walletId, int memberId)
        {
            if (userId == memberId)
            {
                throw new ApiException("You can not remove yourself", 400);
            }
            var owner = await _context.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == userId).FirstOrDefaultAsync();
            if (owner!.Role != MemberRole.Admin)
            {
                throw new ApiException("You are not authorized to delete user", 400);
            }

            var member = await _context.WalletMembers.Where(wM => wM.WalletId == walletId && wM.UserId == memberId).FirstOrDefaultAsync();
            if (member == null)
            {
                throw new ApiException("Member not found", 404);
            }

            _context.WalletMembers.Remove(member);
            await _context.SaveChangesAsync();
        }

        private async Task<List<Notification>> CreateInvitations(int adminId, Wallet wallet, List<int> memberIds)
        {
            var now = DateTime.UtcNow;
            var invitations = memberIds.Select(m => new Invitation
            {
                SenderId = adminId,
                CreatedAt = now,
                UserId = m,
                Status = InvitationStatus.New,
                ExpirationDate = now.AddDays(7),
                WalletId = wallet.Id
            }).ToList();

            await _context.Invitations.AddRangeAsync(invitations);
            await _context.SaveChangesAsync();
            var notis = invitations.Select(i => new Notification
            {
                InvitationId = i.Id,
                UserId = i.UserId,
                Type = NotificationType.JoinWalletInvitation,
                Description = Helper.NotiTemplate.GetInvitationContent(wallet.Name)
            }).ToList();
            await _context.Notifcations.AddRangeAsync(notis);

            return notis;
        }
        private void SendNotifications(List<Notification> notis)
        {
            var notisDtos = _mapper.Map<IEnumerable<NotificationDto>>(notis);
            foreach (var item in notisDtos)
            {
                _ = _notiHub.Clients.Users(item.UserId.ToString()).SendAsync("Notification", item);
            }
        }
        public async Task<bool> VerifyIsUserInWallet(int walletId, int userId)
        {
            var exist = await _context.Wallets.Where(w => w.Id == walletId &&
            w.WalletMembers.Any(m => m.UserId == userId && m.Status == MemberStatus.Accepted)).AnyAsync();

            return exist;
        }
        public async Task<IEnumerable<WalletMember>> GetUsersInWallet(int walletId, int userId)
        {
            var walletMembers = await _context.WalletMembers
            .Where(wM => wM.WalletId == walletId)
            .Include(wM => wM.User).ToListAsync();

            return walletMembers;
        }
        public async Task UpdateWalletBalance(int walletId, int amount)
        {
            var wallet = await _context.Wallets
            .Where(w => w.Id == walletId)
            .FirstOrDefaultAsync();

            if (wallet == null)
            {
                throw new ApiException("Wallet not found!", 400);
            }

            wallet.Balance += amount;
        }
    }
}
