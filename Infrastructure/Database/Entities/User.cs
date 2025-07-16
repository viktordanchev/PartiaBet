using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength()]
        public string Email { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public decimal Balance { get; set; }


    }
}
