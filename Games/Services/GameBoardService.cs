using Core.Enums;
using Core.Games.Models;

namespace Games.Services
{
    public class GameBoardService
    {
        public static GameBoard Create(GameType type)
        {
            return type switch
            {
                GameType.Chess => new GameBoard(),
                //GameType.Backgammon => new BackgammonBoard(),
                //GameType.Belote => new BeloteBoard(),
                //GameType.SixtySix => new SixtySixBoard()
            };
        }
    }
}
