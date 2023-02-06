using System.ComponentModel.DataAnnotations.Schema;
using static MochiApi.Common.Enum;

namespace MochiApi.Dtos
{
    public class CreateWalletMemberDto
    {
        public CreateWalletMemberDto()
        {
            Role = MemberRole.Member;
        }
        public int UserId { get; set; }
        public MemberRole Role { get; set; }
    }
}
