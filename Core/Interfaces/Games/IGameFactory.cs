using Core.Enums;
using Core.Models.Games;

namespace Core.Interfaces.Games
{
    public interface IGameFactory
    {
        IGameService GetGameService(GameType game);
        GameMoveModel GetMakeMoveDto(GameType game, string jsonData);
    }
}
