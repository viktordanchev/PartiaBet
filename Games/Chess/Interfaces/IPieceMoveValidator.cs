using Core.Models.Games.Chess;

namespace Games.Chess.Interfaces
{
    public interface IPieceMoveValidator
    {
        bool IsValidMove(ChessBoardModel board, ChessMoveModel move);
    }
}
