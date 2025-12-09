using Core.Enums;

namespace Core.Dtos.Games
{
    public class GameDto
    {
        public GameType Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
