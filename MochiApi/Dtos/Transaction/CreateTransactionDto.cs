using System.ComponentModel.DataAnnotations;

namespace MochiApi.Dtos
{
    public class CreateTransactionDto
    {
        public CreateTransactionDto()
        {
            Note = String.Empty;
        }
        public int Amount { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public string? Image { get; set; }
        [Required]
        public DateTime? CreatedAt{ get; set; }
        //public int? EventId { get; set; }
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
