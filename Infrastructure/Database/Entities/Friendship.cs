using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class Friendship
    {
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public Guid FriendId { get; set; }

        [ForeignKey(nameof(FriendId))]
        public User Friend { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsAccepted { get; set; }
    }
}
