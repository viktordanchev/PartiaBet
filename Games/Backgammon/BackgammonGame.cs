using Core.Enums;

namespace Games.Backgammon
{
    public class BackgammonGame : Game
    {
        public BackgammonGame()
        {
            GameType = GameType.Backgammon;
            Name = "Backgammon";
            MaxPlayersCount = 2;
        }
    }
}
