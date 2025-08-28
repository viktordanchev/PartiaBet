namespace Core.DTOs
{
    public class UserClaimsDto
    {
        public UserClaimsDto()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; }
    }
}
