using System.ComponentModel.DataAnnotations;

namespace MochiApi.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public UserDto()
        {
            Email = string.Empty;
            Token = string.Empty;
        }
    }
}
