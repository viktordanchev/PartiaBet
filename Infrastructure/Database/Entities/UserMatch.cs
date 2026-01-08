using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class UserMatch
    {
        [Required]
        public Guid MatchId { get; set; }

        [ForeignKey(nameof(MatchId))]
        public Match Match { get; set; } = null!;

        [Required]
        public Guid PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public User Player { get; set; } = null!;

        public int TurnOrder { get; set; }

        public int TeamNumber { get; set; }

        [Required]
        public PlayerStatus Status { get; set; }
    }
}
