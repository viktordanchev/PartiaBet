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
    const [pieceHistory, setPieceHistory] = useState([]);
    
    const getPieceAt = (row, col) => {
        return pieces.find(p => p.row === row && p.col === col) || null;
    };

    const isMyPiece = (piece) => {
        if (!piece) return false;
        
        const pieceType = piece.type.charAt(0);
        
        return pieceType === 'w' && userId === data.whitePlayerId ||
            pieceType === 'b' && userId !== data.whitePlayerId;
    };

    const handleClickSquare = (row, col) => {
        if (selectedPiece && highlightedSquares.some(s => s.newRow === row && s.newCol === col)) {
            setPieceHistory(prev => [...prev, { row, col }]);
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
            setPieceHistory([]);
            var piece = getPieceAt(row, col);
            var cords = getPieceMove(piece, pieces, data.whitePlayerId === userId);

            setPieceHistory(prev => [...prev, { row: piece.row, col: piece.col }]);
            setSelectedPiece(piece);
            setHighlightedSquares(cords || []);
        }
    };

    return (
        <article className="grid grid-cols-8 rounded border-5 border-gray-900">
            {Array.from({ length: 8 * 8 }).map((_, index) => {
                const row = (userId === data.whitePlayerId ? 7 : 0) - Math.floor(index / 8);
                const col = (userId === data.whitePlayerId ? 7 : 0) - index % 8;
                console.log(row, col);
                const isSelected =
                    selectedPiece &&
                    selectedPiece.row === row &&
                    selectedPiece.col === col ||
                    pieceHistory.some(p => p.row === row && p.col === col);

                const piece = getPieceAt(row, col);

                const isHighlighted = highlightedSquares.some(
                    s => s.newRow === row && s.newCol === col
                );
                const isMy = isMyPiece(piece);
                return (
                    <Square
                        key={index}
                        piece={piece ? piece.type : null}
                        isMyPiece={isMy}
                        row={row}
                        col={col}
                        isHighlighted={isHighlighted}
                        selected={isSelected}
                        onSelect={() => (isHighlighted || isMyPiece(piece)) && handleClickSquare(row, col)}
                    />
                );
            })}
        </article>

    );
};

export default Board;
