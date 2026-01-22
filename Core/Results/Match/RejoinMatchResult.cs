using Core.Enums;

namespace Core.Results.Match
{
    public class RejoinMatchResult
    {
        public bool HasChanged { get; private set; }
        public MatchStatus MatchStatus { get; private set; }

        public static RejoinMatchResult Success(bool hasChanged, MatchStatus matchStatus) =>
            new() { HasChanged = hasChanged, MatchStatus = matchStatus };
    }
}
