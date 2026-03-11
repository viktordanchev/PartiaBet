using Core.Models.Match;

namespace Core.Results.Match
{
    public class EndMatchResult
    {
        public Guid MatchId { get; set; }

        public IList<PlayerModel>? Players { get; private set; }

        public static EndMatchResult Success(
            Guid matchId,
            IList<PlayerModel> players) =>
            new()
            {
                MatchId = matchId,
                Players = players
            };
    }
}
