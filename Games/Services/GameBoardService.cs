using Core.Enums;
using Games.Models;

namespace Games.Services
{
    public class GameBoardService
    {
        public static GameBoard Create(Core.Enums.GameType type)
        {
            return type switch
            {
                Core.Enums.GameType.Chess => new GameBoard(),
                //GameType.Backgammon => new BackgammonBoard(),
                //GameType.Belote => new BeloteBoard(),
                //GameType.SixtySix => new SixtySixBoard()
            };
        }
    }
}
