using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class Bet
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        public DateTime DateAndTime { get; set; } = DateTime.Now;

        public TimeSpan Duration { get; set; }

        [Required]
        public string Game { get; set; } = string.Empty;

        [Required]
        public Guid FirstPlayerId { get; set; }

        [ForeignKey(nameof(FirstPlayerId))]
        public User FirstPlayer { get; set; } = null!;

        [Required]
        public Guid SecondPlayerId { get; set; }

        [ForeignKey(nameof(SecondPlayerId))]
        public User SecondPlayer { get; set; } = null!;
    }
}
