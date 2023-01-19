using MochiApi.Models;
using System.ComponentModel.DataAnnotations;
using static MochiApi.Common.Enum;
using static System.Net.WebRequestMethods;

namespace MochiApi.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Icon is required")]
        public CategoryType Type { get; set; }
        public CategoryGroup Group { get; set; }
        public string Icon { get; set; }
        public int? WalletId { get; set; }
        public CreateCategoryDto()
        {
            Name = "";
            Icon = "https://picsum.photos/200";
        }
    }
}
