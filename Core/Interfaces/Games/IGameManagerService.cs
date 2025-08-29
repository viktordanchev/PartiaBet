using Core.Games.Dtos;

namespace Core.Interfaces.Games
{
    public interface IGameManagerService
    {
        void CreateMatch(MatchDto match);
    }
}
