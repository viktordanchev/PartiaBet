using Core.Enums;
using Core.Models.Games;

namespace Core.Results.Match
{
    public class МакеMoveResult
    {
        public bool IsValid { get; private set; }
        public bool IsWinningMove { get; private set; }
        public Guid? WinnerId { get; private set; }
        public Guid NextId { get; set; }
        public double Duration { get; set; }
        public GameType GameType { get; private set; }
        public BaseMoveModel? MoveData { get; private set; }

        public static МакеMoveResult Invalid() =>
            new() { IsValid = false };

        public static МакеMoveResult Success(BaseMoveModel move, GameType gameType) =>
            new() { IsValid = true, MoveData = move, GameType = gameType };

        public static МакеMoveResult Win(Guid winnerId) =>
            new() { IsValid = true, IsWinningMove = true, WinnerId = winnerId };
    }
}
