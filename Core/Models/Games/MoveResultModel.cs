using Core.Enums;

namespace Core.Models.Games
{
    public class MoveResultModel
    {
        public bool IsValid { get; private set; }
        public bool IsWinningMove { get; private set; }
        public Guid? WinnerId { get; private set; }
        public GameType GameType { get; private set; }
        public BaseMoveModel? MoveData { get; private set; }

        public static MoveResultModel Invalid() =>
            new() { IsValid = false };

        public static MoveResultModel Success(BaseMoveModel move, GameType gameType) =>
            new() { IsValid = true, MoveData = move, GameType = gameType };

        public static MoveResultModel Win(Guid winnerId) =>
            new() { IsValid = true, IsWinningMove = true, WinnerId = winnerId };
    }
}
