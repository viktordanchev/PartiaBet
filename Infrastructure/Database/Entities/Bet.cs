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
        public string FirstPlayerId { get; set; } = string.Empty;

        [ForeignKey(nameof(FirstPlayerId))]
        public User FirstPlayer { get; set; } = null!;

        [Required]
        public string SecondPlayerId { get; set; } = string.Empty;

        [ForeignKey(nameof(SecondPlayerId))]
        public User SecondPlayer { get; set; } = null!;
    }
}
