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

        public int TeamNumber { get; set; }

        public bool IsWinner { get; set; }

        public bool IsLeaver { get; set; }
    }
}
