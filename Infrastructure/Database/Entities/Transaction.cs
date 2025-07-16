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
        public TransactionType Type { get; set; }

        [Required]
        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; } = string.Empty;
        public User Receiver { get; set; } = null!;

        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; } = string.Empty;
        public User? Sender { get; set; }
    }
}
