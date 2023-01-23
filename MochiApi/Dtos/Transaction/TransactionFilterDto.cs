namespace MochiApi.Dtos
{
    public class TransactionFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId{ get; set; }
    }
}
