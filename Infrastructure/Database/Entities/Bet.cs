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

        [Required]
        public DateTime Duration { get; set; }

        [Required]
        [ForeignKey(nameof(FirstPlayer))]
        public string FirstPlayerId { get; set; } = string.Empty;
        public User FirstPlayer { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(SecondPlayer))]
        public string SecondPlayerId { get; set; } = string.Empty;
        public User SecondPlayer { get; set; } = null!;
    }
}
