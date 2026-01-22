using Core.Enums;

namespace Core.Results.Match
{
    public class LeaveMatchResult
    {
        public bool IsRemoved { get; private set; }
        public double TimeLeftToRejoin { get; private set; }
        public GameType GameType { get; private set; }

        public static LeaveMatchResult Success(bool isRemoved, GameType gameType, double timeLeft = 0) =>
            new() { IsRemoved = isRemoved, GameType = gameType, TimeLeftToRejoin = timeLeft };
    }
}
