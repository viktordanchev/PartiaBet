namespace Core.DTOs.Shared
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime Penalty { get; set; }

        public decimal Balance { get; set; }
    }
}
