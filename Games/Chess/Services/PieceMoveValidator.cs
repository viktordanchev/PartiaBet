using Core.Games.Enums;
using Games.Chess.Interfaces;
using Games.Chess.PieceMovement.Validators;

namespace Games.Chess.Services
{
    public static class PieceMoveValidator
    {
        private static readonly Dictionary<PieceType, IPieceMoveValidator> _validators
            = new()
            {
            { PieceType.King, new KingMoveValidator() },
            { PieceType.Bishop, new BishopMoveValidator() },
            { PieceType.Rook, new RookMoveValidator() },
            { PieceType.Knight, new KnightMoveValidator() },
            { PieceType.Queen, new QueenMoveValidator() },
            { PieceType.Pawn, new PawnMoveValidator() }
            };

        public static IPieceMoveValidator GetValidator(PieceType type)
        {
            if (_validators.TryGetValue(type, out var validator))
                return validator;

            throw new ArgumentException("Invalid piece type");
        }
    }
}
