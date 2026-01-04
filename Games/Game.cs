using Core.Enums;

namespace Games
{
    public abstract class Game
    {
        public GameType GameType { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImgUrl { get; set; } = string.Empty;

        public int MaxPlayersCount { get; set; }
    }
}
