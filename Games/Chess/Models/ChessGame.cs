using Core.Enums;
using Games.Models;

namespace Games.Chess.Models
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
