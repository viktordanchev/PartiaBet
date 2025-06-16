using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public decimal Balance { get; set; }
    }
}
