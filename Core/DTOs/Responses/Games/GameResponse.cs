namespace Core.DTOs.Responses.Games
{
    public class GameResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MaxPlayers { get; set; }

        public string ImgUrl { get; set; } = string.Empty;
    }
}
