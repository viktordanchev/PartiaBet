using Core.Enums;

namespace Core.Dtos.Games
{
    public class GameDetailsDto
    {
        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public GameType GameType { get; set; }
    }
}
