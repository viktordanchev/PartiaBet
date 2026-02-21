using Core.Enums;

namespace Core.Results.Match
{
    public class LeaveMatchQueueResult
    {
        public Guid MatchId { get; set; }
        public GameType GameType { get; private set; }
        public bool IsRemoved { get; private set; }

        public static LeaveMatchQueueResult Success(Guid matchId, bool isRemoved, GameType gameType) =>
            new() { MatchId = matchId, IsRemoved = isRemoved, GameType = gameType};
    }
}
