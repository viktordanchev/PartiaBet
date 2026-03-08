using Core.Enums;
using Core.Models.Games;
using Core.Models.Match;

namespace Core.Results.Match
{
    public class МакеMoveResult
    {
        public МакеMoveResult()
        {
            Players = new List<PlayerModel>();
        }

        public bool IsValid { get; private set; }
        public bool IsWinningMove { get; private set; }
        public IEnumerable<PlayerModel>? Players { get; private set; }
        public Guid NextId { get; set; }
        public double Duration { get; set; }
        public GameType GameType { get; private set; }
        public GameBoardModel? GameBoard { get; private set; }

        public static МакеMoveResult Invalid() =>
            new() { IsValid = false };

        public static МакеMoveResult Success(GameBoardModel board, GameType gameType) =>
            new() { IsValid = true, GameBoard = board, GameType = gameType };

        public static МакеMoveResult Win(IEnumerable<PlayerModel> winners, GameBoardModel board, GameType gameType) =>
            new() { IsValid = true, IsWinningMove = true, Players = winners, GameBoard = board, GameType = gameType };
    }
}
