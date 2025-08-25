import React from 'react';

const Board = () => {
    const rows = 8;
    const cols = 8;

    return (
        <div className="flex justify-center items-center">
            <div className="grid grid-cols-8 border border-gray-500">
                {Array.from({ length: rows * cols }).map((_, index) => {
                    const row = Math.floor(index / cols);
                    const col = index % cols;
                    const isBlack = (row + col) % 2 === 1;

                    return (
                        <div className={`w-20 h-20 bg-[url('/images/chess/black-king.svg')] bg-contain bg-no-repeat bg-center flex items-center justify-center hover:cursor-pointer hover:bg-yellow-100/90 ${isBlack ? 'bg-gray-600' : 'bg-gray-300'}`}
                            key={index}
                        />
                    );
                })}
            </div>
        </div>
    );
};

export default Board;
