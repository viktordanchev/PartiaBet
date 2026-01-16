function getValidMoves(piece, pieces, isWhite) {
    switch (piece.type) {
        case 'King':
            return getKingMove(piece, pieces, isWhite);
        case 'Queen':
            return getQueenMove(piece, pieces, isWhite);
        case 'Rook':
            return getRookMove(piece, pieces, isWhite);
        case 'Pawn':
            return getPawnMove(piece, pieces, isWhite);
        case 'Bishop':
            return getBishopMove(piece, pieces, isWhite);
        case 'Knight':
            return getKnightMove(piece, pieces, isWhite);
        default:
            return [];
    }
}

function getKingMove(piece, pieces, isWhite) {
    const directions = [
        { row: 1, col: 0 },
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: 0 },
        { row: -1, col: 1 },
        { row: -1, col: -1 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    return getSingleMoves(piece, pieces, isWhite, directions)
}

function getQueenMove(piece, pieces, isWhite) {
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

    return getLinearMoves(piece, pieces, isWhite, directions);
}

function getRookMove(piece, pieces, isWhite) {
    const directions = [
        { row: 1, col: 0 },
        { row: -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    return getLinearMoves(piece, pieces, isWhite, directions);
}

function getPawnMove(piece, pieces, isWhite) {
    const validSquares = [];
    const directions = [
        { row: isWhite ? 1 : -1, col: 0 },
        { row: isWhite ? 1 : -1, col: 1 },
        { row: isWhite ? 1 : -1, col: -1 }
    ];

    if (piece.row === 1 || piece.row === 6) {
        directions.push({ row: isWhite ? 2 : -2, col: 0 });
    }

    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;

        if (row >= 0 && row < 8 && col >= 0 && col < 8) {
            const piece = pieces.find(p => p.row === row && p.col === col);

            if ((dir.col !== 0 && piece) ||
                (dir.col === 0 && !piece)) {
                if (piece) {
                    if (piece.isWhite !== isWhite) {
                        validSquares.push({ row, col });
                    }
                } else {
                    validSquares.push({ row, col });
                }
            }
        }
    }

    return validSquares;
}

function getBishopMove(piece, pieces, isWhite) {
    const directions = [
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: -1 },
        { row: -1, col: 1 }
    ];

    return getLinearMoves(piece, pieces, isWhite, directions);
}

function getKnightMove(piece, pieces, isWhite) {
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

    return getSingleMoves(piece, pieces, isWhite, directions);
}

function getLinearMoves(piece, pieces, isWhite, directions) {
    var validSquares = [];

    for (const dir of directions) {
        for (let i = 1; i < 8; i++) {
            const row = piece.row + dir.row * i;
            const col = piece.col + dir.col * i;

            if (row < 0 || row >= 8 || col < 0 || col >= 8) continue;

            const pieceInSquare = pieces.find(p => p.row === row && p.col === col);

            if (pieceInSquare) {
                if (pieceInSquare.isWhite !== isWhite) {
                    validSquares.push({ row, col });
                }

                break;
            } else {
                validSquares.push({ row, col });
            }
        }
    }

    return validSquares;
}

function getSingleMoves(piece, pieces, isWhite, directions) {
    const validSquares = [];

    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;
        
        if (row < 0 || row >= 8 || col < 0 || col >= 8) continue;

        const pieceInSquare = pieces.find(p => p.row === row && p.col === col);
        
        if (pieceInSquare) {
            if (pieceInSquare.isWhite !== isWhite) {
                validSquares.push({ row, col });
            }
        } else {
            validSquares.push({ row, col });
        }
    }

    return validSquares;
}


export default getValidMoves;