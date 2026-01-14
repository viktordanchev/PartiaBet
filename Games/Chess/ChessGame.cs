using Core.Enums;

namespace Games.Chess
{
    public class ChessGame : Game
    {
        public ChessGame()
        {
            GameType = GameType.Chess;
            Name = "Chess";
            MaxPlayersCount = 2;
            TeamsCount = 2;
        }
    }
}
