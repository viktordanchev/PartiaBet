using Core.Enums;

namespace Core.Models.Match
{
    public class MatchStatusModel
    {
        public bool IsStarted { get; private set; }
        public GameType GameType { get; private set; }
        public PlayerModel AddedPlayer { get; private set; }

        public static MatchStatusModel Success(PlayerModel addedPlayer, GameType gameType, bool isStarted) =>
            new() { AddedPlayer = addedPlayer, GameType = gameType, IsStarted = isStarted };
    }
}
