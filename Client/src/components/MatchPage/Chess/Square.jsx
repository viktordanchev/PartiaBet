import React from 'react';
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

const Square = ({ square, isClickable, isHighlighted, selected, onSelect }) => {
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
        <div className={`relative w-20 h-20 flex items-center justify-center
        ${(isClickable && square.type) && 'hover:bg-yellow-100'}
        ${(isClickable && square || isHighlighted) && 'hover:cursor-pointer'}
        ${(square.row + square.col) % 2 === 1 ? 'bg-gray-600' : 'bg-gray-300'}
        ${selected && 'bg-yellow-100'}`}
            onClick={() => onSelect()}
        >
            <img src={pieces[square.type]} className="h-full" />
            {isHighlighted && !square.type && (
                <div className="absolute w-5 h-5 bg-yellow-100 rounded-full" />
            )}
        </div>
    );
};

export default Square;