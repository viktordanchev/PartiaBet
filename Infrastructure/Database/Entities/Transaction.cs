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

        public DateTime DateAndTime { get; set; } = DateTime.Now;

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        [Required]  
        public TransactionType Type { get; set; }

        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [ForeignKey(nameof(ReceiverId))]
        public User Receiver { get; set; } = null!;

        public string SenderId { get; set; } = string.Empty;

        [ForeignKey(nameof(SenderId))]
        public User? Sender { get; set; }
    }
}
