using Core.Enums;

namespace Core.Results.Match
{
    public class PlayerRejoinMatchResult
    {
        public Guid MatchId { get; private set; }
        public MatchStatus MatchStatus { get; private set; }

        public static PlayerRejoinMatchResult Success(Guid matchId, MatchStatus matchStatus) =>
            new() { MatchId = matchId, MatchStatus = matchStatus };
    }
}
