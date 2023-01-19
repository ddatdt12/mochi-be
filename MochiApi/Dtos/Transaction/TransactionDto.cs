namespace MochiApi.Dtos
{
    public class TransactionDto : BaseEntityDto
    {
        public TransactionDto()
        {
            Note = String.Empty;
        }
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Note { get; set; }
        public int CreatorId { get; set; }
        public UserDto? Creator { get; set; }
        public int WalletId { get; set; }
        public WalletDto? Wallet { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public int? EventId { get; set; }

    }
}
