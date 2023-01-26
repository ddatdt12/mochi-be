using MochiApi.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Models
{
    public class UpdateEventDto
    {
        public UpdateEventDto()
        {
            Name = null!;
            Icon = null!;
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Icon { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
