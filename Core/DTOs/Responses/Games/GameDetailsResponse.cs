using Core.Enums;

namespace Core.DTOs.Responses.Games
{
    public class GameDetailsResponse
    {
        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public GameType GameType { get; set; }
    }
}
