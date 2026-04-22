namespace Core.Results.Match
{
    public class HandlePlayerDisconnectResult
    {
        public bool IsSuccess { get; set; }
        public Guid MatchId { get; set; }
        public DateTime? RejoinDeadline { get; set; }

        public static HandlePlayerDisconnectResult Success(Guid matchId, DateTime? rejoinDeadline) =>
            new() { IsSuccess = true, MatchId = matchId, RejoinDeadline = rejoinDeadline };

        public static HandlePlayerDisconnectResult Invalid() =>
            new() { IsSuccess = false };
    }
}
