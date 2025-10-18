using Core.Enums;

namespace Core.DTOs.Responses.Games
{
    public class GameResponse
    {
        public GameType Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
