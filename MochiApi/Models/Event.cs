﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Models
{
    [Table("Event")]
    public class Event : BaseEntity
    { 
        public Event()
        {
            Name = null!;
            Icon = null!;
            Transactions = new List<Transaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime? EndDate{ get; set; }
        public int CreatorId { get; set; }
        public User? Creator { get; set; }
        public int? WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public bool IsFinished { get; set; }
        public ICollection<Transaction> Transactions{ get; set; }
    }
}
