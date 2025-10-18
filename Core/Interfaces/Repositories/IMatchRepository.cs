using Core.DTOs.Requests.Matches;

namespace Core.Interfaces.Repositories
{
    public interface IMatchRepository
    {
        Task<Guid> AddMatch(AddMatchRequest match);
    }
}
