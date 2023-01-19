namespace MochiApi.Dtos
{
    public class UpdateTransactionDto
    {
        public UpdateTransactionDto()
        {
            Note = String.Empty;
        }
        public int? Amount { get; set; }
        public string? Note { get; set; }
        public int? CategoryId { get; set; }
        public int? EventId { get; set; }

    }
}
