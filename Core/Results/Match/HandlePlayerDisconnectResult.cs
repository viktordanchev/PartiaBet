namespace Core.Results.Match
{
    public class HandlePlayerDisconnectResult
    {
        public Guid MatchId { get; set; }
        public double TimeLeftToRejoin { get; set; }

        public static HandlePlayerDisconnectResult Success(Guid matchId, double timeLeftToRejoin) =>
            new() { MatchId = matchId, TimeLeftToRejoin = timeLeftToRejoin };
    }
}
