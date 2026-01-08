using Core.Enums;

namespace Games.Chess
{
    public class ChessGame : Game
    {
        public ChessGame()
        {
            GameType = GameType.Chess;
            Name = "Chess";
            ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/chess.jpg";
            MaxPlayersCount = 2;
            TeamsCount = 2;
        }
    }
}
