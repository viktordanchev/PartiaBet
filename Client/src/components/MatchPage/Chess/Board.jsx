import React, { useState } from 'react';
import { useParams } from "react-router-dom";
import { jwtDecode } from 'jwt-decode';
import Square from './Square';
import getValidMoves from '../../../services/chess/getValidMoves';
import { useMatchHub } from '../../../contexts/MatchHubContext';

const Board = ({ data, isMyTurn }) => {
    const { matchId } = useParams();
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const { connection } = useMatchHub();
    const [selectedPiece, setSelectedPiece] = useState(null);
    const [validSquares, setValidSquares] = useState([]);
    const isHostWhite = data.whitePlayerId === decodedToken['Id'];

    const isClickable = (square) => {
        if (!square.type) return false;

        const isOwnPiece = square.isWhite === isHostWhite;
        const isValidMoveTarget = validSquares.some(
            vs => vs.row === square.row && vs.col === square.col
        );

        return isOwnPiece || isValidMoveTarget;
    };

    const makeMove = async (moveData) => {
        try {
            await connection.invoke("MakeMove", matchId, JSON.stringify(moveData));
        } catch (error) {
            console.error(error);
        }
    };

    const handleClickSquare = async (piece) => {
        if (!isMyTurn) return;

        const isMove =
            selectedPiece &&
            validSquares.some(s => s.row === piece.row && s.col === piece.col);

        if (isMove) {
            setSelectedPiece(null);
            setValidSquares([]);

            await makeMove(
                {
                    oldRow: selectedPiece.row,
                    oldCol: selectedPiece.col,
                    newRow: piece.row,
                    newCol: piece.col
                });
        } else {
            var validMoves = getValidMoves(piece, data.pieces, isHostWhite);

            setSelectedPiece(piece);
            setValidSquares(validMoves);
        }
    };
    
    return (
        <article className={`grid grid-cols-8 rounded border-5 border-gray-900
        ${!isMyTurn && 'pointer-events-none'}`}>
            {Array.from({ length: 8 * 8 }).map((_, index) => {
                const [row, col] = [Math.floor(index / 8), index % 8].map(v => (isHostWhite ? 7 - v : v));
                
                const square = data.pieces.find(p => p.row === row && p.col === col) || { row, col };
                
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
