using Core.Enums;
using Games.Models;

namespace Games.Belote
{
    public class BeloteGame : Game
    {
        public BeloteGame()
        {
            GameType = GameType.Belote;
            Name = "Belote";
            MaxPlayersCount = 4;
        }
    }
}
