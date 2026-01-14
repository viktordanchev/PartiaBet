using Core.Enums;

namespace Games
{
    public abstract class Game
    {
        public GameType GameType { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MaxPlayersCount { get; set; }

        public int TeamsCount { get; set; }
    }
}
