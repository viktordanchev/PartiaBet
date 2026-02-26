using Microsoft.AspNetCore.Http;

namespace Core.Models.User
{
    public class UpdateUserModel
    {
        public string Username { get; set; } = string.Empty;

        public IFormFile? ProfileImage { get; set; }

        public string CurrentPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
