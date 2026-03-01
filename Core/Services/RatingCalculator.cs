using Core.Interfaces.Services;
using Core.Models.Match;

namespace Core.Services
{
    public class RatingCalculator : IRatingCalculator
    {
        private const int RatingChange = 30;

        public void CalculatePlayersRating(MatchModel match)
        {
            foreach (var player in match.Players)
            {
                if (player.Status == Enums.PlayerStatus.Winner)
                {
                    player.Rating += RatingChange;
                }
                else
                {
                    player.Rating -= RatingChange;
                }
            }
        }
    }
}
