using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class UserGameRating
    {
        [Required]
        public Guid PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public User Player { get; set; } = null!;

        [Required]
        public GameType GameType { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
