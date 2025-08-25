import React from 'react';
import BlackKing from '../../../assets/images/chess/black-king.svg';

const Piece = ({ piece, pieceRow, pieceCol, row, col }) => {
    const pieces = {
        'bk': BlackKing,
    };

    return (
        <div className={`w-20 h-20 flex items-center justify-center hover:cursor-pointer hover:bg-yellow-100/90 focus:bg-yellow-100/90 ${(row + col) % 2 === 1 ? 'bg-gray-600' : 'bg-gray-300'}`}>
            {(pieceRow === row && pieceCol === col) && (<img src={pieces[piece]} className="h-full " />)}
        </div>
    );
};

export default Piece;