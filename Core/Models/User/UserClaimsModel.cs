namespace Core.Models.User
{
    public class UserClaimsModel
    {
        public UserClaimsModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; }
    }
}
