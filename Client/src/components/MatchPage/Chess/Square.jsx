import React, { useState } from 'react';
import BlackBishop from '../../../assets/images/chess/black-bishop.svg';
import BlackKing from '../../../assets/images/chess/black-king.svg';
import BlackKnight from '../../../assets/images/chess/black-knight.svg';
import BlackPawn from '../../../assets/images/chess/black-pawn.svg';
import BlackQueen from '../../../assets/images/chess/black-queen.svg';
import BlackRook from '../../../assets/images/chess/black-rook.svg';
import WhiteBishop from '../../../assets/images/chess/white-bishop.svg';
import WhiteKing from '../../../assets/images/chess/white-king.svg';
import WhiteKnight from '../../../assets/images/chess/white-knight.svg';
import WhitePawn from '../../../assets/images/chess/white-pawn.svg';
import WhiteQueen from '../../../assets/images/chess/white-queen.svg';
import WhiteRook from '../../../assets/images/chess/white-rook.svg';

const Square = ({ piece, pieceRow, pieceCol, row, col }) => {
    const [selected, setSelected] = useState(false);

    const pieces = {
        'bbishop': BlackBishop,
        'bking': BlackKing,
        'bknight': BlackKnight,
        'bpawn': BlackPawn,
        'bqueen': BlackQueen,
        'brook': BlackRook,
        'wbishop': WhiteBishop,
        'wking': WhiteKing,
        'wknight': WhiteKnight,
        'wpawn': WhitePawn,
        'wqueen': WhiteQueen,
        'wrook': WhiteRook,
    };

    return (
        <div className={`w-20 h-20 flex items-center justify-center hover:cursor-pointer hover:bg-yellow-100/90 
        ${(row + col) % 2 === 1 ? 'bg-gray-600' : 'bg-gray-300'}
        ${selected && 'bg-yellow-100/90'}`}
            onClick={() => setSelected(!selected)}
        >
            {(pieceRow === row && pieceCol === col) && (<img src={pieces[piece]} className="h-full" />)}
        </div>
    );
};

export default Square;