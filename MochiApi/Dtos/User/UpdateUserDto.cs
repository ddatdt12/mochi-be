using System.ComponentModel.DataAnnotations;

namespace MochiApi.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        public string Avatar { get; set; }
        public UpdateUserDto()
        {
        }
    }
}
