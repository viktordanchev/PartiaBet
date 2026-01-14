using Core.Enums;

namespace Core.Models.Games
{
    public class GameModel
    {
        public GameType GameType { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MaxPlayersCount { get; set; }
    }
}
