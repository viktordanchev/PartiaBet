using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class Friendship
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public string FriendId { get; set; }
        public User Friend { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsAccepted { get; set; }
    }
}
