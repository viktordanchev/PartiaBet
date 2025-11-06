function getPieceMove(piece, pieces) {
    switch (piece.type.slice(1)) {
        case 'king':
            return getKingMove(piece, pieces);
        case 'rook':
            return getRookMove(piece, pieces);
        default:
            return [];
    }
}

function getKingMove(piece, pieces) {
    var validSquares = [];
    const directions = [
        { row: 1, col: 0 },
        { row: -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    for (const dir of directions) {
        let newRow = piece.row + dir.row;
        let newCol = piece.col + dir.col;
        
        if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
            validSquares.push({ newRow, newCol });
        }
    }

    return validSquares;
}

function getRookMove(piece, pieces) {
    var validSquares = [];
    const directions = [
        { row: 1, col: 0 },
        { row: -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    for (const dir of directions) {
        for (let i = 1; i < 8; i++) {
            let newRow = piece.row + dir.row * i;
            let newCol = piece.col + dir.col * i;

            if (newRow < 0 || newRow >= 8 || newCol < 0 || newCol >= 8) {
                break;
            } else if (!pieces.some(p => p.row === newRow && p.col === newCol)) {
                validSquares.push({ newRow, newCol });
            } else {
                break;
            }
        }
    }

    return validSquares;
}

function getPawnMove(piece, pieces) {
    var validSquares = [];
    const directions = [
        { row: 1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    for (const dir of directions) {
        let newRow = piece.row + dir.row;
        let newCol = piece.col + dir.col;

        if (piece.row === 1 && dir.row === 6) {

        }

        if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
            validSquares.push({ newRow, newCol });
        }
    }

    return validSquares;
}

export default getPieceMove;