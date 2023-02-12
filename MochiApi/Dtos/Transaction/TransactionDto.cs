namespace MochiApi.Dtos
{
    public class TransactionDto
    {
        public TransactionDto()
        {
            Note = String.Empty;
            Participants = new List<UserDto>();
            UnknownParticipants = new List<string>();
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
        public DateTime CreatedAt { get; set; }
        public int? EventId { get; set; }
        public EventDto? Event { get; set; }
        public string? Image { get; set; }
        public List<UserDto> Participants { get; set; }

        public int? RelevantTransactionId { get; set; }
        public TransactionDto? RelevantTransaction { get; set; }
        public List<string> UnknownParticipants { get; set; }

        public List<TransactionDto> ChildTransactions { get; set; }
        public int AccumulatedAmount { get; set; }
        public long ChildAmountSum { get; set; }
    }
}
