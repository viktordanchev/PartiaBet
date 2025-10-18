using Core.DTOs.Requests.Matches;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<Guid> AddMatchAsync(AddMatchRequest match);
    }
}
