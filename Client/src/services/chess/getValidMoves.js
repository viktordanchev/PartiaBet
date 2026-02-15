function getValidMoves(board, piece, pieces) {
    switch (piece.type) {
        case 'King':
            return getKingMove(board, piece, pieces);
        case 'Queen':
            return getQueenMove(piece, pieces);
        case 'Rook':
            return getRookMove(piece, pieces);
        case 'Pawn':
            return getPawnMove(piece, pieces);
        case 'Bishop':
            return getBishopMove(piece, pieces);
        case 'Knight':
            return getKnightMove(piece, pieces);
        default:
            return [];
    }
}

function getKingMove(board, piece, pieces) {
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

    const validSquares = getSingleMoves(piece, pieces, directions);
    const smallCastleType = piece.isWhite ? board.canWhiteSmallCastle : board.canBlackSmallCastle;
    const bigCastleType = piece.isWhite ? board.canWhiteBigCastle : board.canBlackBigCastle;

    if (smallCastleType) {
        const rookKingSide = pieces.find(
            p => p.type === 'Rook' &&
                p.row === backRank &&
                p.col === 7 &&
                p.isWhite === piece.isWhite
        );

        if (rookKingSide) {
            const square5 = pieces.find(p => p.row === backRank && p.col === 5);
            const square6 = pieces.find(p => p.row === backRank && p.col === 6);

            if (!square5 && !square6) {
                validSquares.push({ row: backRank, col: 6 });
            }
        }

        const rookQueenSide = pieces.find(
            p => p.type === 'Rook' &&
                p.row === backRank &&
                p.col === 0 &&
                p.isWhite === piece.isWhite
        );

        if (rookQueenSide) {
            const square1 = pieces.find(p => p.row === backRank && p.col === 1);
            const square2 = pieces.find(p => p.row === backRank && p.col === 2);
            const square3 = pieces.find(p => p.row === backRank && p.col === 3);

            if (!square1 && !square2 && !square3) {
                validSquares.push({ row: backRank, col: 2 });
            }
        }
    }

    return validSquares;
}

function getQueenMove(piece, pieces) {
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

    return getLinearMoves(piece, pieces, directions);
}

function getRookMove(piece, pieces) {
    const directions = [
        { row: 1, col: 0 },
        { row: -1, col: 0 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];

    return getLinearMoves(piece, pieces, directions);
}

function getPawnMove(piece, pieces) {
    const validSquares = [];
    const directions = [
        { row: piece.isWhite ? 1 : -1, col: 0 },
        { row: piece.isWhite ? 1 : -1, col: 1 },
        { row: piece.isWhite ? 1 : -1, col: -1 }
    ];

    if (piece.row === 1 || piece.row === 6) {
        directions.unshift({ row: piece.isWhite ? 2 : -2, col: 0 });
    }
    
    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;
        
        if (row < 0 || row > 7 || col < 0 || col > 7) continue;
        
        const square = pieces.find(p => p.row === row && p.col === col);
        
        if (square && dir.col === 0) continue;
        
        if (dir.col !== 0 && (!square || (square && square.isWhite === piece.isWhite))) continue;

        validSquares.push({ row, col });
    }

    return validSquares;
}

function getBishopMove(piece, pieces) {
    const directions = [
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: -1 },
        { row: -1, col: 1 }
    ];

    return getLinearMoves(piece, pieces, directions);
}

function getKnightMove(piece, pieces) {
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

    return getSingleMoves(piece, pieces, directions);
}

function getLinearMoves(piece, pieces, directions) {
    var validSquares = [];

    for (const dir of directions) {
        for (let i = 1; i < 8; i++) {
            const row = piece.row + dir.row * i;
            const col = piece.col + dir.col * i;

            if (row < 0 || row >= 8 || col < 0 || col >= 8) continue;

            const pieceInSquare = pieces.find(p => p.row === row && p.col === col);

            if (pieceInSquare) {
                if (pieceInSquare.isWhite !== piece.isWhite) {
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

function getSingleMoves(piece, pieces, directions) {
    const validSquares = [];

    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;
        
        if (row < 0 || row >= 8 || col < 0 || col >= 8) continue;

        const pieceInSquare = pieces.find(p => p.row === row && p.col === col);
        
        if (pieceInSquare) {
            if (pieceInSquare.isWhite !== piece.isWhite) {
                validSquares.push({ row, col });
            }
        } else {
            validSquares.push({ row, col });
        }
    }

    return validSquares;
}


export default getValidMoves;