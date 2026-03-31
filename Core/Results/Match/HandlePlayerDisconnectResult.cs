namespace Core.Results.Match
{
    public class HandlePlayerDisconnectResult
    {
        public bool IsSuccess { get; set; }
        public Guid MatchId { get; set; }
        public double TimeLeftToRejoin { get; set; }

        public static HandlePlayerDisconnectResult Success(Guid matchId, double timeLeftToRejoin) =>
            new() { IsSuccess = true, MatchId = matchId, TimeLeftToRejoin = timeLeftToRejoin };

        public static HandlePlayerDisconnectResult Invalid() =>
            new() { IsSuccess = false };
    }
}
