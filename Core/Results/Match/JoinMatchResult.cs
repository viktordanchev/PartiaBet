using Core.Enums;
using Core.Models.Match;

namespace Core.Results.Match
{
    public class JoinMatchResult
    {
        public bool IsValid { get; private set; }
        public bool IsStarted { get; private set; }
        public GameType GameType { get; private set; }
        public PlayerModel AddedPlayer { get; private set; }

        public static JoinMatchResult Invalid() =>
            new() { IsValid = false };

        public static JoinMatchResult Success(PlayerModel addedPlayer, GameType gameType, bool isStarted) =>
            new() { AddedPlayer = addedPlayer, GameType = gameType, IsStarted = isStarted };
    }
}
