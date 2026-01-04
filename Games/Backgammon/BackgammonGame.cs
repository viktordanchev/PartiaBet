using Core.Enums;

namespace Games.Backgammon
{
    public class BackgammonGame : Game
    {
        public BackgammonGame()
        {
            GameType = GameType.Backgammon;
            Name = "Backgammon";
            ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/backgammon.png";
            MaxPlayersCount = 2;
        }
    }
}
