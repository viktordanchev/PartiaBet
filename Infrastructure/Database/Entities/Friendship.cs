using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class Friendship
    {
        [Required]
        public Guid FirstUserId { get; set; }

        [ForeignKey(nameof(FirstUserId))]
        public User FirstUser { get; set; } = null!;

        [Required]
        public Guid SecondUserId { get; set; }

        [ForeignKey(nameof(SecondUserId))]
        public User SecondUser { get; set; } = null!;

        [Required]
        public Guid RequesterId { get; set; }

        public DateTime CreatedAt { get; set; }

        public FriendshipStatus Status { get; set; }
    }
}
