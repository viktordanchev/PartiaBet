using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IRatingCalculator
    {
        void CalculatePlayersRating(MatchModel match);
    }
}
