using Common.Constants;
using RestAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Dtos.User
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(Validations.User.UsernameMaxLength)]
        public string Username { get; set; } = string.Empty;

        public IFormFile? ProfileImage { get; set; }

        [Validate(nameof(NewPassword))]
        public string CurrentPassword { get; set; } = string.Empty;

        [Validate(nameof(CurrentPassword))]
        public string NewPassword { get; set; } = string.Empty;

        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}