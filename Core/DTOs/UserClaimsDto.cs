namespace Core.DTOs
{
    public class UserClaimsDto
    {
        public UserClaimsDto()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
