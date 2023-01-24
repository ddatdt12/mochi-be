using System.ComponentModel.DataAnnotations;

namespace MochiApi.Dtos
{
    public class BasicUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public BasicUserDto()
        {
            Email = string.Empty;
        }
    }
}
