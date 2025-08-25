import React from 'react';

const Piece = () => {
    return (
        <div className={`w-20 h-20 bg-[url('/images/chess/black-king.svg')] bg-contain bg-no-repeat bg-center flex items-center justify-center hover:cursor-pointer hover:bg-yellow-100/90 ${isBlack ? 'bg-gray-600' : 'bg-gray-300'}`}
            key={index}
        />
    );
};

export default Piece;