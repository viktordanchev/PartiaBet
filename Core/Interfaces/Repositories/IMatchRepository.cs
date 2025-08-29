using Core.Games.Dtos;

namespace Core.Interfaces.Repositories
{
    public interface IMatchRepository
    {
        Task<Guid> AddMatch(MatchDto match);
    }
}
