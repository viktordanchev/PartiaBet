using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Games
{
    public interface IGameFactory
    {
        IGameService GetGameService(GameType game);
        BaseMoveDto GetMakeMoveDto(GameType game, string jsonData);
    }
}
