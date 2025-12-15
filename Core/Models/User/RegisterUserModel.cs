namespace Core.Models.User
{
    public class RegisterUserModel
    {
        public string Email { get; set; } = string.Empty;
        public string VrfCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DateAndTime { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
