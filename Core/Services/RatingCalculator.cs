using Core.Enums;
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
                if (player.Result == MatchResult.Win)
                {
                    player.NewRating = player.CurrentRating + RatingChange;
                }
                else
                {
                    player.NewRating = player.CurrentRating - RatingChange;

                    if(player.NewRating < 0)
                        player.NewRating = 0;
                }
            }
        }
    }
}
