import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import Square from './Square';
import getValidMoves from '../../../services/chess/getValidMoves';
import { useHub } from '../../../contexts/HubContext';

const Board = ({ data }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const { connection } = useHub();
    const [pieces, setPieces] = useState(data.pieces);
    const [selectedPiece, setSelectedPiece] = useState(null);
    const [validSquares, setValidSquares] = useState([]);
    const isHostWhite = data.whitePlayerId === decodedToken['Id'];
   
    useEffect(() => {
        if (!connection) return;
        
        const handleReceiveMove = (data) => {
            setPieces(prev =>
                prev.map(p => p.row === data.oldRow && p.col === data.oldCol ? { ...p, row: data.newRow, col: data.newCol } : p)
            );
        };

        connection.on("ReceiveMove", handleReceiveMove);

        return () => {
            connection.off("ReceiveMove", handleReceiveMove);
        };
    }, [connection]);

    const isClickable = (square) => {
        if (!square.type) return false;

        return square.isWhite && isHostWhite ||
            !square.isWhite && !isHostWhite ||
            validSquares.some(vs => vs.row === square.row && vs.col === square.col);
    };

    const makeMove = async (moveData) => {
        const matchId = sessionStorage.getItem('currentMatchId');

        try {
            await connection.invoke("MakeMove", matchId, JSON.stringify(moveData));
        } catch (error) {
            console.error(error);
        }
    };

    const handleClickSquare = async (piece) => {
        if (selectedPiece && validSquares.some(s => s.row === piece.row && s.col === piece.col)) {
            setPieces(prev =>
                prev.map(p =>
                    p.row === selectedPiece.row && p.col === selectedPiece.col
                        ? { ...p, row: piece.row, col: piece.col }
                        : p
                )
            );
            setSelectedPiece(null);
            setValidSquares([]);

            await makeMove(
                {
                    oldRow: selectedPiece.row,
                    oldCol: selectedPiece.col,
                    newRow: piece.row,
                    newCol: piece.col,
                    pieceType:selectedPiece.type
                });
        } else {
            var validMoves = getValidMoves(piece, pieces, isHostWhite);

            setSelectedPiece(piece);
            setValidSquares(validMoves);
        }
    };

    return (
        <article className="grid grid-cols-8 rounded border-5 border-gray-900">
            {Array.from({ length: 8 * 8 }).map((_, index) => {
                const [row, col] = [Math.floor(index / 8), index % 8].map(v => (isHostWhite ? 7 - v : v));

                const square = pieces.find(p => p.row === row && p.col === col) || { row, col };

                const isHighlighted = validSquares.some(
                    vs => vs.row === row && vs.col === col
                );

                const isSquareClickable = isClickable(square);

                return (
                    <Square
                        key={index}
                        square={square}
                        isClickable={isSquareClickable || isHighlighted}
                        isHighlighted={isHighlighted}
                        isSelected={selectedPiece && selectedPiece.row === row && selectedPiece.col === col}
                        onSelect={() => (isHighlighted || isSquareClickable) && handleClickSquare(square)}
                    />
                );
            })}
        </article>

    );
};

export default Board;
