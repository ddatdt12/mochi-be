using static MochiApi.Common.Enum;

namespace MochiApi.Dtos
{
    public class CreateWalletDto
    {
        public CreateWalletDto()
        {
            Name = string.Empty;
            Icon = string.Empty;
            MemberIds = new List<int>();
        }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Balance { get; set; }
        public int? ClonedCategoryWalletId { get; set; }
        public WalletType Type { get; set; }
        public ICollection<int> MemberIds { get; set; }

    }
}
