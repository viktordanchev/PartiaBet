namespace Core.Models.Match
{
    public class AddPlayerModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
