using MochiApi.Models;
using System.ComponentModel.DataAnnotations;
using static MochiApi.Common.Enum;
using static System.Net.WebRequestMethods;

namespace MochiApi.Dtos
{
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public CategoryType? Type { get; set; }
        public CategoryGroup? Group { get; set; }

        public string? Icon { get; set; }
        public UpdateCategoryDto()
        {
            Name = "";
            Icon = "https://picsum.photos/200";
        }
    }
}
