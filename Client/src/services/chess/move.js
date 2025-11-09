function getPieceMove(piece, pieces, isRotated) {
    switch (piece.type.slice(1)) {
        case 'king':
            return getKingMove(piece, pieces);
        case 'queen':
            return getQueenMove(piece, pieces);
        case 'rook':
            return getRookMove(piece, pieces);
        case 'pawn':
            return getPawnMove(piece, pieces, isRotated);
        case 'bishop':
            return getBishopMove(piece, pieces);
        case 'knight':
            return getKnightMove(piece, pieces);
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

function getQueenMove(piece, pieces) {
    var validSquares = [];
    const directions = [
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: -1 },
        { row: -1, col: 1 },
        { row: 1, col: 0 },
        { row: -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];


    for (const dir of directions) {
        for (let i = 1; i < 8; i++) {
            let newRow = piece.row + dir.row * i;
            let newCol = piece.col + dir.col * i;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
                validSquares.push({ newRow, newCol });
            } else {
                break;
            }
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

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
                validSquares.push({ newRow, newCol });
            } else {
                break;
            }
        }
    }

    return validSquares;
}

function getPawnMove(piece, pieces, isRotated) {
    var validSquares = [];
    const directions = [
        { row: isRotated ? 1 : -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    for (const dir of directions) {
        if (dir.col === 1 || dir.col === -1) {
            continue;
        }

        let newRow = piece.row + dir.row;
        let newCol = piece.col + dir.col;

        if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
            validSquares.push({ newRow, newCol });
        }

        if (piece.row === 1 || piece.row === 6) {
            newRow = isRotated ? ++newRow : --newRow;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
                validSquares.push({ newRow, newCol });
            }
        }
    }

    return validSquares;
}

function getBishopMove(piece, pieces) {
    var validSquares = [];
    const directions = [
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: -1 },
        { row: -1, col: 1 }
    ];

    for (const dir of directions) {
        for (let i = 1; i < 8; i++) {
            let newRow = piece.row + dir.row * i;
            let newCol = piece.col + dir.col * i;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !pieces.some(p => p.row === newRow && p.col === newCol)) {
                validSquares.push({ newRow, newCol });
            } else {
                break;
            }
        }
    }

    return validSquares;
}

function getKnightMove(piece, pieces) {
    var validSquares = [];
    const directions = [
        { row: 2, col: 1 },
        { row: 2, col: -1 },
        { row: -2, col: 1 },
        { row: -2, col: -1 },
        { row: 1, col: 2 },
        { row: -1, col: 2 },
        { row: 1, col: -2 },
        { row: -1, col: -2 }
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


export default getPieceMove;