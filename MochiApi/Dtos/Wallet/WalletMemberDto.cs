using System.ComponentModel.DataAnnotations.Schema;
using static MochiApi.Common.Enum;

namespace MochiApi.Dtos
{
    public class WalletMemberDto
    {
        public WalletMemberDto()
        {
            Role = MemberRole.Member;
            JoinAt = DateTime.UtcNow;
            Status = MemberStatus.Pending;
        }
        public int UserId { get; set; }
        public UserDto? User { get; set; }
        public int WalletId { get; set; }
        public WalletDto? Wallet { get; set; }
        public MemberStatus Status { get; set; }
        public MemberRole Role { get; set; }
        public DateTime JoinAt { get; set; }

    }
}
