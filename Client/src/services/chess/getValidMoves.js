function getValidMoves(piece, pieces, areWhitePiecesMine) {
    switch (piece.type) {
        case 'king':
            return getKingMove(piece, pieces, areWhitePiecesMine);
        case 'queen':
            return getQueenMove(piece, pieces, areWhitePiecesMine);
        case 'rook':
            return getRookMove(piece, pieces, areWhitePiecesMine);
        case 'pawn':
            return getPawnMove(piece, pieces, areWhitePiecesMine);
        case 'bishop':
            return getBishopMove(piece, pieces, areWhitePiecesMine);
        case 'knight':
            return getKnightMove(piece, pieces, areWhitePiecesMine);
        default:
            return [];
    }
}

function getKingMove(piece, pieces, areWhitePiecesMine) {
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

    return getSingleMoves(piece, pieces, areWhitePiecesMine, directions)
}

function getQueenMove(piece, pieces, areWhitePiecesMine) {
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

    return getLinearMoves(piece, pieces, areWhitePiecesMine, directions);
}

function getRookMove(piece, pieces, areWhitePiecesMine) {
    const directions = [
        { row: 1, col: 0 },
        { row: -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    return getLinearMoves(piece, pieces, areWhitePiecesMine, directions);
}

function getPawnMove(piece, pieces, areWhitePiecesMine) {
    const validSquares = [];
    const directions = [
        { row: areWhitePiecesMine ? 1 : -1, col: 0 },
        { row: areWhitePiecesMine ? 1 : -1, col: 1 },
        { row: areWhitePiecesMine ? 1 : -1, col: -1 }
    ];

    if (piece.row === 1 || piece.row === 6) {
        directions.push({ row: areWhitePiecesMine ? 2 : -2, col: 0 });
    }
    
    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;

        if (row >= 0 && row < 8 && col >= 0 && col < 8) {
            const piece = pieces.find(p => p.row === row && p.col === col);

            if ((dir.col !== 0 && piece) ||
                (dir.col === 0 && !piece)) {
                if (piece) {
                    validSquares.push({ row, col });
                } else {
                    validSquares.push({ row, col });
                }
            }
        }
    }

    return validSquares;
}

function getBishopMove(piece, pieces, areWhitePiecesMine) {
    const directions = [
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: -1 },
        { row: -1, col: 1 }
    ];

    return getLinearMoves(piece, pieces, areWhitePiecesMine, directions);
}

function getKnightMove(piece, pieces, areWhitePiecesMine) {
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

    return getSingleMoves(piece, pieces, areWhitePiecesMine, directions);
}

function getLinearMoves(piece, pieces, areWhitePiecesMine, directions) {
    var validSquares = [];

    for (const dir of directions) {
        for (let i = 1; i < 8; i++) {
            const row = piece.row + dir.row * i;
            const col = piece.col + dir.col * i;

            if (row >= 0 && row < 8 && col >= 0 && col < 8) {
                const piece = pieces.find(p => p.row === row && p.col === col);

                if (piece) {
                    if (piece.isWhite !== areWhitePiecesMine) {
                        validSquares.push({ row, col });
                    }

                    break;
                } else {
                    validSquares.push({ row, col });
                }
            }
        }
    }

    return validSquares;
}

function getSingleMoves(piece, pieces, areWhitePiecesMine, directions) {
    const validSquares = [];

    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;

        if (row >= 0 && row < 8 && col >= 0 && col < 8) {
            const piece = pieces.find(p => p.row === row && p.col === col);

            if (piece) {
                if (piece.isWhite !== areWhitePiecesMine) {
                    validSquares.push({ row, col });
                }
            } else {
                validSquares.push({ row, col });
            }
        }
    }

    return validSquares;
}


export default getValidMoves;