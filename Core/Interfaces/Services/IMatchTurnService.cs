using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchTurnService
    {
        void StartTurn(MatchModel match, PlayerModel player);
        void EndTurn(MatchModel match, PlayerModel player);
    }
}
