using System.ComponentModel.DataAnnotations;

namespace MochiApi.Dtos
{
    public class UpdateTransactionDto
    {
        public UpdateTransactionDto()
        {
            Note = String.Empty;
            ParticipantIds = new List<int>();
            UnknownParticipants = new List<string>();
        }
        public int Amount { get; set; }
        [Required]
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public int? EventId { get; set; }
        public string? Image { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        public List<int> ParticipantIds { get; set; }
        public int? RelevantTransactionId { get; set; }
        public List<string> UnknownParticipants { get; set; }
        public DateTime CreateAtValue
        {
            get
            {
                if (!CreatedAt.HasValue)
                {
                    return DateTime.UtcNow;
                }
                return (DateTime)CreatedAt!;
            }
        }
    }
}
