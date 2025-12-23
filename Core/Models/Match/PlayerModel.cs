namespace Core.Models.Match
{
    public class PlayerModel
    {
        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int Rating { get; set; }
    }
}
