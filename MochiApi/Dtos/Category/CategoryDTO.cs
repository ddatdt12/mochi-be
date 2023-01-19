using MochiApi.Models;
using System.ComponentModel.DataAnnotations;
using static MochiApi.Common.Enum;
using static System.Net.WebRequestMethods;

namespace MochiApi.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Icon { get; set; }
        public CategoryType Type { get; set; }
        public CategoryGroup Group { get; set; }
        public int? WalletId { get; set; }
        public WalletDto? Wallet { get; set; }
        public CategoryDto()
        {
            Name = "";
            Icon = "https://picsum.photos/200";
            Wallet = null;
        }
    }
}
