using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class Match
    {
        public Match()
        {
            Players = new List<UserMatch>();
        }

        [Key]
        public Guid Id { get; set; }

        public decimal BetAmount { get; set; }

        public DateTime DateAndTime { get; set; }

        [Required]
        public MatchStatus MatchStatus { get; set; }

        [Required]
        public GameType GameType { get; set; }

        public ICollection<UserMatch> Players { get; set; } = new List<UserMatch>();
    }
}
