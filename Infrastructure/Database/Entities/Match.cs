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

        [Required]
        public DateTime StartTimeUTC { get; set; }

        public DateTime EndTimeUTC { get; set; }

        [Required]
        public GameType GameType { get; set; }

        public ICollection<UserMatch> Players { get; set; } = new List<UserMatch>();
    }
}
