namespace MochiApi.Dtos
{
    public class TransactionGroupDateDto
    {
        public TransactionGroupDateDto()
        {
            Transactions = new List<TransactionDto>();
        }
        public DateTime Date{ get; set; }
        public long Revenue{ get; set; }
        public List<TransactionDto> Transactions{ get; set; }

    }
}
