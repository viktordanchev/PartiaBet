using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class Friendship
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Friend))]
        public Guid FriendId { get; set; }
        public User Friend { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsAccepted { get; set; }
    }
}
