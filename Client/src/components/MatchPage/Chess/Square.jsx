import React from 'react';

const Square = ({ skin, square, isClickable, isHighlighted, selected, onSelect }) => {
    return (
        <div className={`relative w-20 h-20 flex items-center justify-center
        ${(isClickable && square.type) && 'hover:bg-yellow-100'}
        ${(isClickable && square || isHighlighted) && 'hover:cursor-pointer'}
        ${(square.row + square.col) % 2 === 1 ? 'bg-gray-600' : 'bg-gray-300'}
        ${selected && 'bg-yellow-100'}`}
            onClick={() => onSelect()}
        >
            <img src={skin && skin} className="h-full" />
            {isHighlighted && !square.type && (
                <div className="absolute w-5 h-5 bg-yellow-100 rounded-full" />
            )}
        </div>
    );
};

export default Square;