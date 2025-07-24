using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class User
    {
        public User()
        {
            SentFriendRequests = new List<Friendship>();
            ReceivedFriendRequests = new List<Friendship>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength()]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public DateTime Penalty { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Friendship> SentFriendRequests { get; set; }
        public ICollection<Friendship> ReceivedFriendRequests { get; set; }
    }
}
