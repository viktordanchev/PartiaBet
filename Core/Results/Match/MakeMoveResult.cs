using Core.Enums;
using Core.Models.Games;

namespace Core.Results.Match
{
    public class MakeMoveResult
    {
        public MoveStatus Status { get; private set; }

        public GameBoardModel? GameBoard { get; private set; }

        public Guid? NextPlayerId { get; set; }

        public double? Duration { get; set; }

        public static MakeMoveResult Invalid() =>
            new()
            {
                Status = MoveStatus.Invalid
            };

        public static MakeMoveResult Success(GameBoardModel board, MoveStatus status) =>
            new()
            {
                Status = status,
                GameBoard = board,
            };
    }
}
