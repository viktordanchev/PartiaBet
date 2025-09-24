namespace Core.Games.Dtos
{
    public class PlayerDto
    {
        public Guid Id { get; set; }

        public string ProfileImgUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int Rating { get; set; }

        public bool IsActivePlayer { get; set; }
    }
}
