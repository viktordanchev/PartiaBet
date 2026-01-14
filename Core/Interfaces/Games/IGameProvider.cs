using Core.Enums;
using Core.Models.Games;

namespace Core.Interfaces.Games
{
    public interface IGameProvider
    {
        IEnumerable<GameModel> GenerateAllGames();
        int GetMaxPlayersCount(GameType gameType);
        bool IsValidGameType(GameType gameType);
    }
}
