import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import Square from './Square';
import getPieceMove from '../../../services/chess/move';

const Board = ({ data }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const [pieces, setPieces] = useState(data.pieces);
    const [selectedPiece, setSelectedPiece] = useState(null);
    const [highlightedSquares, setHighlightedSquares] = useState([]);

    const getPieceAt = (row, col) => {
        return pieces.find(p => p.row === row && p.col === col) || null;
    };

    const handleClickSquare = (row, col) => {
        if (selectedPiece) {
            setPieces(prev =>
                prev.map(p =>
                    p.row === selectedPiece.row && p.col === selectedPiece.col
                        ? { ...p, row: row, col: col }
                        : p
                )
            );
            setSelectedPiece(null);
            setHighlightedSquares([]);
        } else {
            var piece = getPieceAt(row, col);
            var cords = getPieceMove(piece, pieces);

            setSelectedPiece(piece);
            setHighlightedSquares(cords || []);
        }
    };

    return (
        <article className="grid grid-cols-8 rounded border-5 border-gray-900">
            {Array.from({ length: 8 * 8 }).map((_, index) => {
                const row = Math.floor(index / 8);
                const col = index % 8;
                const renderRow = userId === data.whitePlayerId ? 7 - row : row;
                const renderCol = userId === data.whitePlayerId ? 7 - col : col;

                const isSelected =
                    selectedPiece &&
                    selectedPiece.row === renderRow &&
                    selectedPiece.col === renderCol;

                const isHighlighted = highlightedSquares.some(
                    s => s.newRow === renderRow && s.newCol === renderCol
                );

                const piece = getPieceAt(renderRow, renderCol);
                
                return (
                    <Square
                        key={index}
                        piece={piece ? piece.type : null}
                        row={renderRow}
                        col={renderCol}
                        isHighlighted={isHighlighted}
                        selected={isSelected}
                        onSelect={() => handleClickSquare(renderRow, renderCol)}
                    />
                );
            })}
        </article>

    );
};

export default Board;
