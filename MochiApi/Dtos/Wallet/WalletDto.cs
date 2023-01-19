using static MochiApi.Common.Enum;

namespace MochiApi.Dtos
{
    public class WalletDto
    {
        public WalletDto()
        {
            Name = string.Empty;
            Icon = string.Empty;
            Members = new List<UserDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Balance { get; set; }
        public bool IsDefault { get; set; }
        public WalletType Type { get; set; }
        public ICollection<UserDto> Members { get; set; }

    }
}
