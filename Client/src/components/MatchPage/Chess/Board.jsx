import React, { useState } from 'react';
import Square from './Square';

const Board = () => {
    const [selectedCell, setSelectedCell] = useState(null);
    const rows = 8;
    const cols = 8;

    return (
        <article className="grid grid-cols-8 rounded border border-gray-500">
            {Array.from({ length: rows * cols }).map((_, index) => {
                return (
                    <Square
                        key={index}
                        piece={'bk'}
                        pieceRow={4}
                        pieceCol={2}
                        row={Math.floor(index / cols)}
                        col={index % cols}
                        onSelect={() => setSelectedCell({ row, col })}
                    />
                );
            })}
        </article>
    );
};

export default Board;
