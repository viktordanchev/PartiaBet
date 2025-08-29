using Core.Games.Dtos;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task AddMatchAsync(MatchDto match);
    }
}
