import React, { useState } from 'react';
import BlackKing from '../../../assets/images/chess/black-king.svg';

const Square = ({ piece, pieceRow, pieceCol, row, col }) => {
    const [selected, setSelected] = useState(false);

    const pieces = {
        'bk': BlackKing,
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