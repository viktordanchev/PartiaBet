using Core.Games.Enums;
using Core.Interfaces.Games;
using Core.Models.Games;
using Core.Models.Games.Chess;
using Core.Models.Match;
using Games.Chess.Models;
using System.ComponentModel;
using System.IO.Pipelines;

namespace Games.Chess.Services
{
    public class ChessService : IGameService
    {
        private readonly ChessGame _gameConfig;

        public ChessService()
        {
            _gameConfig = new ChessGame();
        }

        public GameBoardModel CreateGameBoard(IEnumerable<PlayerModel> players)
        {
            var chessBoard = new ChessBoardModel();

            InitializePieces(chessBoard, true);
            InitializePieces(chessBoard, false);

            foreach (var player in players)
            {
                UpdatePlayersInBoard(chessBoard, player.Id);

                if (player.Id == chessBoard.WhitePlayerId)
                {
                    player.IsOnTurn = true;
                }
            }

            chessBoard.CanBlackBigCastle= true;
            chessBoard.CanBlackSmallCastle= true;
            chessBoard.CanWhiteBigCastle= true;
            chessBoard.CanWhiteSmallCastle= true;

            return chessBoard;
        }

        public Guid SwitchTurn(Guid playerId, IEnumerable<PlayerModel> players)
        {
            var newTurnPlayerId = Guid.Empty;
            var currentPlayer = players.First(p => p.Id == playerId);

            if (_gameConfig.MaxPlayersCount == currentPlayer.TurnOrder)
            {
                newTurnPlayerId = players.First(p => p.TurnOrder == 1).Id;
            }
            else
            {
                newTurnPlayerId = players.First(p => p.TurnOrder == currentPlayer.TurnOrder + 1).Id;
            }

            return newTurnPlayerId;
        }

        public void UpdateBoard(GameBoardModel board, GameMoveModel move)
        {
            var chessBoard = board as ChessBoardModel;
            var chessMove = move as ChessMoveModel;
            var currPiece = chessBoard.Pieces.FirstOrDefault(p => p.Row == chessMove.OldRow && p.Col == chessMove.OldCol);
            var targetPiece = chessBoard.Pieces.FirstOrDefault(p => p.Row == chessMove.NewRow && p.Col == chessMove.NewCol);

            if (ChessMoveService.IsCastleMove(chessBoard, currPiece, targetPiece))
            {
                ChessMoveService.PerformCastle(chessBoard, currPiece, targetPiece);
                return;
            }

            if (targetPiece != null)
            {
                chessBoard.Pieces.Remove(targetPiece);
            }

            currPiece.Row = chessMove.NewRow;
            currPiece.Col = chessMove.NewCol;
        }

        public bool IsWinningMove(GameBoardModel board)
        {
            var chessBoard = board as ChessBoardModel;

            return ChessMoveService.IsWinningMove(chessBoard);
        }

        public bool IsValidMove(GameBoardModel board, GameMoveModel move)
        {
            var chessBoard = board as ChessBoardModel;
            var chessMove = move as ChessMoveModel;

            return ChessMoveService.IsValidMove(chessBoard, chessMove);
        }

        //private methods

        private void UpdatePlayersInBoard(ChessBoardModel board, Guid playerId)
        {
            if (board.WhitePlayerId == Guid.Empty && board.BlackPlayerId == Guid.Empty)
            {
                bool isWhite = Random.Shared.Next(0, 2) == 0;

                if (isWhite)
                {
                    board.WhitePlayerId = playerId;
                }
                else
                {
                    board.BlackPlayerId = playerId;
                }

                return;
            }

            if (board.WhitePlayerId != Guid.Empty && board.BlackPlayerId == Guid.Empty)
            {
                board.BlackPlayerId = playerId;
                return;
            }

            if (board.BlackPlayerId != Guid.Empty && board.WhitePlayerId == Guid.Empty)
            {
                board.WhitePlayerId = playerId;
                return;
            }
        }

        private void InitializePieces(ChessBoardModel board, bool isWhite)
        {
            for (int col = 0; col < 8; col++)
            {
                board.Pieces.Add(new FigureModel() { Type = PieceType.Pawn, Row = isWhite ? 1 : 6, Col = col, IsWhite = isWhite });
            }

            var row = isWhite ? 0 : 7;

            board.Pieces.Add(new FigureModel() { Type = PieceType.Rook, Row = row, Col = 0, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Rook, Row = row, Col = 7, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Knight, Row = row, Col = 1, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Knight, Row = row, Col = 6, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Bishop, Row = row, Col = 2, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Bishop, Row = row, Col = 5, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.King, Row = row, Col = 3, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Queen, Row = row, Col = 4, IsWhite = isWhite });
        }
    }
}