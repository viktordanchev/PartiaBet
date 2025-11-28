import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import Square from './Square';
import getPieceMove from '../../../services/chess/move';
import { useHub } from '../../../contexts/HubContext';

const Board = ({ data }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const playerId = decodedToken['Id'];
    const { connection } = useHub();
    const [pieces, setPieces] = useState(data.pieces);
    const [selectedPiece, setSelectedPiece] = useState(null);
    const [highlightedSquares, setHighlightedSquares] = useState([]);
    
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

    const getPieceAt = (row, col) => {
        return pieces.find(p => p.row === row && p.col === col) || null;
    };

    const isMyPiece = (piece) => {
        if (!piece.type) return false;

        const pieceType = piece.type.charAt(0);

        return pieceType === 'w' && playerId === data.whitePlayerId ||
            pieceType === 'b' && playerId !== data.whitePlayerId;
    };

    const makeMove = async (oldRow, oldCol, row, col) => {
        var jsonData = {
            oldRow: oldRow,
            oldCol: oldCol,
            newRow: row,
            newCol: col
        };

        const matchId = sessionStorage.getItem('currentMatchId');
        
        try {
            await connection.invoke("MakeMove", matchId, playerId, JSON.stringify(jsonData));
        } catch (error) {
            console.error(error);
        }
    };

    const handleClickSquare = async (row, col) => {
        if (selectedPiece && highlightedSquares.some(s => s.newRow === row && s.newCol === col)) {
            setPieces(prev =>
                prev.map(p =>
                    p.row === selectedPiece.row && p.col === selectedPiece.col
                        ? { ...p, row: row, col: col }
                        : p
                )
            );
            setSelectedPiece(null);
            setHighlightedSquares([]);
            
            await makeMove(selectedPiece.row, selectedPiece.col, row, col);
        } else {
            var piece = getPieceAt(row, col);
            var cords = getPieceMove(piece, pieces, data.whitePlayerId === playerId);

            setSelectedPiece(piece);
            setHighlightedSquares(cords || []);
        }
    };

    return (
        <article className="grid grid-cols-8 rounded border-5 border-gray-900">
            {Array.from({ length: 8 * 8 }).map((_, index) => {
                const [row, col] = [Math.floor(index / 8), index % 8].map(v => (playerId === data.whitePlayerId ? 7 - v : v));

                const piece = getPieceAt(row, col) || { row, col };

                const isHighlighted = highlightedSquares.some(
                    s => s.newRow === row && s.newCol === col
                );

                const isClickable = isMyPiece(piece);

                return (
                    <Square
                        key={index}
                        square={piece}
                        isClickable={isClickable}
                        isHighlighted={isHighlighted}
                        selected={selectedPiece && selectedPiece.row === row && selectedPiece.col === col}
                        onSelect={() => (isHighlighted || isClickable) && handleClickSquare(row, col)}
                    />
                );
            })}
        </article>

    );
};

export default Board;
