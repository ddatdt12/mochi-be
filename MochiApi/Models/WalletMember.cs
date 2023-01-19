using System.ComponentModel.DataAnnotations.Schema;
using static MochiApi.Common.Enum;

namespace MochiApi.Models
{
    [Table("WalletMember")]
    public class WalletMember
    {
        public WalletMember()
        {
            Role = MemberRole.Member;
            JoinAt = DateTime.UtcNow;
        }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public MemberRole Role { get; set; }
        public DateTime JoinAt { get; set; }

    }
}
