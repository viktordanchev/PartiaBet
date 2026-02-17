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

    let validSquares = getSingleMoves(piece, pieces, directions);

    const smallCastleType = piece.isWhite
        ? board.canWhiteSmallCastle
        : board.canBlackSmallCastle;

    const bigCastleType = piece.isWhite
        ? board.canWhiteBigCastle
        : board.canBlackBigCastle;

    if (smallCastleType) {
        addCastleMove(piece, pieces, validSquares, -1);
    }

    if (bigCastleType) {
        addCastleMove(piece, pieces, validSquares, 1);
    }

    validSquares = getAttackedSquares(validSquares, board, piece.isWhite);

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
        { row: piece.isWhite ? 1 : -1, col: 1 },
        { row: piece.isWhite ? 1 : -1, col: -1 },
        { row: piece.isWhite ? 1 : -1, col: 0 }
    ];

    if (piece.row === 1 || piece.row === 6) {
        directions.push({ row: piece.isWhite ? 2 : -2, col: 0 });
    }

    for (const dir of directions) {
        const row = piece.row + dir.row;
        const col = piece.col + dir.col;

        if (row < 0 || row > 7 || col < 0 || col > 7) continue;

        const square = pieces.find(p => p.row === row && p.col === col);

        if (square) {
            if (dir.col !== 0 && square.isWhite === piece.isWhite) {
                continue;
            } else if (dir.col === 0) {
                break
            }
        } else {
            if (dir.col !== 0) {
                continue;
            }
        }

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

function addCastleMove(piece, pieces, validSquares, direction) {
    const start = piece.col + direction;
    const end = direction === -1 ? 0 : 7;

    for (let i = start; direction === -1 ? i >= end : i <= end; i += direction) {
        const pieceOnSquare = pieces.find(
            p => p.row === piece.row && p.col === i
        );

        if (pieceOnSquare && pieceOnSquare.type === 'Rook' && piece.isWhite === pieceOnSquare.isWhite) {
            validSquares.push({ row: piece.row, col: i });
            break;
        } else if (pieceOnSquare) {
            break;
        }
    }
}

function getAttackedSquares(validSquares, board, isWhite) {

    const enemyPieces = board.pieces.filter(p => p.isWhite !== isWhite);
    let attackedSquares = [];

    for (const piece of enemyPieces) {

        let attackMoves = [];

        switch (piece.type) {

            case 'Queen':
                attackMoves = getQueenMove(piece, board.pieces);
                break;

            case 'Rook':
                attackMoves = getRookMove(piece, board.pieces);
                break;

            case 'Bishop':
                attackMoves = getBishopMove(piece, board.pieces);
                break;

            case 'Knight':
                attackMoves = getKnightMove(piece, board.pieces);
                break;

            case 'Pawn':
                attackMoves = [
                    {
                        row: piece.row + (piece.isWhite ? 1 : -1),
                        col: piece.col + 1
                    },
                    {
                        row: piece.row + (piece.isWhite ? 1 : -1),
                        col: piece.col - 1
                    }
                ].filter(s =>
                    s.row >= 0 && s.row < 8 &&
                    s.col >= 0 && s.col < 8
                );
                break;

            case 'King':
                attackMoves = getSingleMoves(piece, board.pieces, [
                    { row: 1, col: 0 },
                    { row: 1, col: 1 },
                    { row: 1, col: -1 },
                    { row: -1, col: 0 },
                    { row: -1, col: 1 },
                    { row: -1, col: -1 },
                    { row: 0, col: 1 },
                    { row: 0, col: -1 }
                ]);
                break;

            default:
                attackMoves = [];
        }

        attackedSquares.push(...attackMoves);
    }

    return validSquares.filter(square =>
        !attackedSquares.some(attacked =>
            attacked.row === square.row &&
            attacked.col === square.col
        )
    );
}

export default getValidMoves;