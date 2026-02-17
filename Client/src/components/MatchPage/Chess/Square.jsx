import React from 'react';
import getFigureImage from '../../../constants/chessFigureImages';

const Square = ({ square, isClickable, isHighlighted, isSelected, onSelect }) => {
    const isEmpty = !square.type;
    
    return (
        <div className={`relative w-20 h-20 flex items-center justify-center
        ${isClickable && 'hover:bg-yellow-100 hover:cursor-pointer'}
        ${(square.row + square.col) % 2 === 1 ? 'bg-gray-600' : 'bg-gray-300'}
        ${isSelected && 'bg-yellow-100'}
        ${(!isEmpty && isHighlighted) && 'bg-green-400'}`}
            onClick={() => onSelect()}
        >
            {isEmpty ?
                <>
                    {isHighlighted && (<div className="absolute w-5 h-5 bg-yellow-100 rounded-full" />)}
                </> :
                <img src={getFigureImage(square.type, square.isWhite)} className="h-full" />
            }
        </div>
    );
};

export default Square;