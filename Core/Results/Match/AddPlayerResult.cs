using Core.Enums;
using Core.Models.Match;

namespace Core.Results.Match
{
    public class AddPlayerResult
    {
        public bool IsStarted { get; private set; }
        public GameType GameType { get; private set; }
        public PlayerModel AddedPlayer { get; private set; }

        public static AddPlayerResult Success(PlayerModel addedPlayer, GameType gameType, bool isStarted) =>
            new() { AddedPlayer = addedPlayer, GameType = gameType, IsStarted = isStarted };
    }
}
