using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class Match
    {
        public Match()
        {
            Players = new List<UserMatch>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal BetAmount { get; set; }

        public DateTime DateAndTime { get; set; }

        public TimeSpan Duration { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

        public ICollection<UserMatch> Players { get; set; } = new List<UserMatch>();
    }
}
