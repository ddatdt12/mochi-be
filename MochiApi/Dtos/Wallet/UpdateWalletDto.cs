using static MochiApi.Common.Enum;

namespace MochiApi.Dtos
{
    public class UpdateWalletDto
    {
        public UpdateWalletDto()
        {
            Name = string.Empty;
            Icon = string.Empty;
        }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public int? Balance { get; set; }
        public WalletType? Type { get; set; }
    }
}
