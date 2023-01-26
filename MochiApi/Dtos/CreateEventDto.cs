using MochiApi.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Models
{
    public class CreateEventDto
    {
        public CreateEventDto()
        {
            Name = null!;
            Icon = null!;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime? EndDate { get; set; }
        public int CreatorId { get; set; }
        public int? WalletId { get; set; }
    }
}
