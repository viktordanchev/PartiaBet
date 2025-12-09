import React from 'react';

const Square = ({ square, isClickable, isHighlighted, isSelected, onSelect }) => {
    const emptySquare = !square.type;

    return (
        <div className={`relative w-20 h-20 flex items-center justify-center
        ${isClickable && 'hover:bg-yellow-100 hover:cursor-pointer'}
        ${(square.row + square.col) % 2 === 1 ? 'bg-gray-600' : 'bg-gray-300'}
        ${isSelected && 'bg-yellow-100'}
        ${(!emptySquare && isHighlighted) && 'bg-green-400'}`}
            onClick={() => onSelect()}
        >
            <img src={square.imageUrl && square.imageUrl} className="h-full" />
            {isHighlighted && emptySquare && (
                <div className="absolute w-5 h-5 bg-yellow-100 rounded-full" />
            )}
        </div>
    );
};

export default Square;