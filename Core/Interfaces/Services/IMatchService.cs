using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(AddMatchModel data);
        Task<PlayerModel> AddPersonToMatch(Guid matchId, Guid playerId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId);
        Task<MatchDetailsModel> GetMatch(Guid matchId);
        Task TryMakeMove(Guid matchId, GameType game, string playerId, BaseMoveModel moveData);
        Task<GameType> GetMatchGameTypeAsync(Guid matchId);
    }
}
