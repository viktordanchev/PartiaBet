using Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Requests.Account
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(Validations.User.EmailMaxLength)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(Validations.User.UsernameMaxLength)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public DateTime RegisteredAt { get; set; }
    }
}
