using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;

namespace Infrastructure.Database.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DateTimeUTC { get; set; } = DateTime.UtcNow;

        [Required]
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        [Required]  
        public TransactionType Type { get; set; }

        [Required]
        public TransactionMethod TransactionMethod { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
