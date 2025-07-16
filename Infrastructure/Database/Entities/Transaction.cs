using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DTOs.Enums;

namespace Infrastructure.Database.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        public DateTime DateAndTime { get; set; } = DateTime.UtcNow;

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        [Required]
        [ForeignKey(nameof(User))]
        public string ReceiverId { get; set; } = string.Empty;
        public User Receiver { get; set; } = null!;
    }
}
