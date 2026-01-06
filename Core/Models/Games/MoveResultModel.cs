namespace Core.Models.Games
{
    public class MoveResultModel
    {
        public bool IsValid { get; private set; }
        public bool IsWinningMove { get; private set; }
        public Guid? WinnerId { get; private set; }
        public BaseMoveModel Move { get; private set; }

        public static MoveResultModel Invalid(BaseMoveModel move) =>
            new() { IsValid = false, Move = move };

        public static MoveResultModel Success(BaseMoveModel move) =>
            new() { IsValid = true, Move = move };

        public static MoveResultModel Win(BaseMoveModel move, Guid winnerId) =>
            new() { IsValid = true, IsWinningMove = true, WinnerId = winnerId, Move = move };
    }
}
