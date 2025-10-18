namespace Games.Dtos.Response
{
    public class PlayerResponse
    {
        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int Rating { get; set; }

        public int Team { get; set; }

        public bool IsActivePlayer { get; set; }
    }
}
