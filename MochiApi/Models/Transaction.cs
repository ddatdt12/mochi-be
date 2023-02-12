using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Models
{
    public class Transaction
    {
        public Transaction()
        {
            Note = String.Empty;
            ParticipantIds = String.Empty;
            Participants = new List<User>();
            CreatedAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Note { get; set; }
        public int CreatorId { get; set; }
        public User? Creator { get; set; }
        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? EventId { get; set; }
        public Event? Event { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public string? Image { get; set; }
        public int? RelevantTransactionId { get; set; }
        public int AccumulatedAmount { get; set; }
        public Transaction? RelevantTransaction { get; set; }
        [Column("varchar(100)")]
        public string UnknownParticipantsStr { get; set; }
        [NotMapped]
        public List<string> UnknownParticipants
        {
            get
            {
                return UnknownParticipantsStr.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            set
            {
                UnknownParticipantsStr = string.Join(";", value);
            }
        }
        public string ParticipantIds { get; set; }
        [NotMapped]
        public List<User> Participants { get; set; }
        [NotMapped]
        public List<Transaction> ChildTransactions { get; set; }
    }
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasIndex(t => t.UnknownParticipantsStr);
        }
    }
}
