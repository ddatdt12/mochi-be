using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Models
{
    [Table("User")]
    public class User
    {
        public User()
        {
            Email = string.Empty;
            Password = string.Empty;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Settings Settings{ get; set; }
        public ICollection<Wallet> Wallets { get; set; }
        public ICollection<WalletMember> WalletMembers { get; set; }
    }


    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
             .HasMany(p => p.Wallets)
             .WithMany(p => p.Members)
             .UsingEntity<WalletMember>(
             r => r.HasOne(rm => rm.Wallet).WithMany(u => u.WalletMembers).HasForeignKey(rm => rm.UserId),
             r => r.HasOne(rm => rm.User).WithMany(u => u.WalletMembers).HasForeignKey(rm => rm.WalletId),
             rm =>
             {
                 rm.HasKey(t => new { t.UserId, t.WalletId });
             }
             );
        }
    }
}