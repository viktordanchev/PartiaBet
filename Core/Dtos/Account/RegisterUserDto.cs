using Common.Constants;
using Core.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Account
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        [StringLength(Validations.User.EmailMaxLength)]
        public string Email { get; set; } = string.Empty;

        [RequiredInProduction]
        public string VrfCode { get; set; } = string.Empty;

        [Required]
        [StringLength(Validations.User.UsernameMaxLength)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string DateAndTime { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
