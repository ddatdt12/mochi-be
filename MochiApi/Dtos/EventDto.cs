using MochiApi.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Models
{
    public class EventDto : BaseEntity
    {
        public EventDto()
        {
            Name = null!;
            Icon = null!;
            Transactions = new List<Transaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public long Spent { get; set; }
        public DateTime? EndDate { get; set; }
        public int CreatorId { get; set; }
        public UserDto? Creator { get; set; }
        public int? WalletId { get; set; }
        public WalletDto? Wallet { get; set; }
        public bool IsFinished { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
