using Core.Enums;
using Games.Models;

namespace Games.SixtySix
{
    public class SixtySixGame : Game
    {
        public SixtySixGame()
        {
            GameType = GameType.SixtySix;
            Name = "Sixty-Six";
            MaxPlayersCount = 2;
        }
    }
}
