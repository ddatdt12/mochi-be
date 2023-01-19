﻿using System.ComponentModel.DataAnnotations;

namespace MochiApi.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<UserDto> Members{     get; set; }
        public UserDto()
        {
            Email = string.Empty;
            Members = new List<UserDto>();
        }
    }
}
