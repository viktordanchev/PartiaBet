using Core.Enums;

namespace RestAPI.Dtos.Games
{
    public class GameDto
    {
        public GameType GameType { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MaxPlayersCount { get; set; }
    }
}
