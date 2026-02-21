function getValidMoves(board, piece) {
    let validMoves = [];
    
    switch (piece.type) {
        case 'King':
            validMoves = getKingMove(board, piece);
            break;
        case 'Queen':
            validMoves = getQueenMove(piece, board.pieces);
            break;
        case 'Rook':
            validMoves = getRookMove(piece, board.pieces);
            break;
        case 'Pawn':
            validMoves = getPawnMove(piece, board.pieces);
            break;
        case 'Bishop':
            validMoves = getBishopMove(piece, board.pieces);
            break;
        case 'Knight':
            validMoves = getKnightMove(piece, board.pieces);
            break;
        default:
            return validMoves;
    }

    validMoves = filterMovesLeavingKingInCheck(board, piece, validMoves);

    return validMoves
}

function getKingMove(board, piece) {
    const validSquares = getSafeKingMoves(board, piece, piece.isWhite);

    const smallCastleType = piece.isWhite
        ? board.canWhiteSmallCastle
        : board.canBlackSmallCastle;

    const bigCastleType = piece.isWhite
        ? board.canWhiteBigCastle
        : board.canBlackBigCastle;

    if (smallCastleType) {
        addCastleMove(piece, board.pieces, validSquares, -1);
    }

    if (bigCastleType) {
        addCastleMove(piece, board.pieces, validSquares, 1);
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

function getSafeKingMoves(board, kingPiece, isKingWhite) {
    const enemyPieces = board.pieces.filter(p => p.isWhite !== isKingWhite);
    let attackedSquares = [];
    const kingDirections = [
        { row: 1, col: 0 },
        { row: 1, col: 1 },
        { row: 1, col: -1 },
        { row: -1, col: 0 },
        { row: -1, col: 1 },
        { row: -1, col: -1 },
        { row: 0, col: 1 },
        { row: 0, col: -1 }
    ];
    let kingMoves = getSingleMoves(kingPiece, board.pieces, kingDirections);

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
                attackMoves = getSingleMoves(piece, board.pieces, kingDirections);
                break;

            default:
                attackMoves = [];
        }

        attackedSquares.push(...attackMoves);
    }

    return kingMoves.filter(square =>
        !attackedSquares.some(attacked =>
            attacked.row === square.row &&
            attacked.col === square.col
        )
    );
}

function filterMovesLeavingKingInCheck(board, piece, validMoves) {

    const legalMoves = [];

    for (const move of validMoves) {

        // 1️⃣ Deep copy на board
        const simulatedBoard = structuredClone(board);

        // 2️⃣ Намери фигурата в копието
        const simulatedPiece = simulatedBoard.pieces.find(
            p => p.row === piece.row &&
                p.col === piece.col &&
                p.type === piece.type &&
                p.isWhite === piece.isWhite
        );

        if (!simulatedPiece) continue;

        // 3️⃣ Махни евентуално взета фигура
        simulatedBoard.pieces = simulatedBoard.pieces.filter(
            p => !(p.row === move.row && p.col === move.col)
        );

        // 4️⃣ Премести фигурата
        simulatedPiece.row = move.row;
        simulatedPiece.col = move.col;

        // 5️⃣ Провери дали царят е в шах
        if (!isKingInCheck(simulatedBoard, piece.isWhite)) {
            legalMoves.push(move);
        }
    }

    return legalMoves;
}

function isKingInCheck(board, isWhite) {
    const king = board.pieces.find(
        p => p.type === 'King' && p.isWhite === isWhite
    );

    if (!king) return false;

    const enemyPieces = board.pieces.filter(
        p => p.isWhite !== isWhite
    );

    for (const enemy of enemyPieces) {

        const attackMoves = getPseudoMoves(board, enemy);

        if (attackMoves.some(
            move => move.row === king.row && move.col === king.col
        )) {
            return true;
        }
    }

    return false;
}

function getPseudoMoves(board, piece) {
    switch (piece.type) {
        case 'Queen':
            return getQueenMove(piece, board.pieces);
        case 'Rook':
            return getRookMove(piece, board.pieces);
        case 'Bishop':
            return getBishopMove(piece, board.pieces);
        case 'Knight':
            return getKnightMove(piece, board.pieces);
        case 'Pawn':
            return getPawnMove(piece, board.pieces);
        case 'King':
            return getSingleMoves(piece, board.pieces, [
                { row: 1, col: 0 },
                { row: 1, col: 1 },
                { row: 1, col: -1 },
                { row: -1, col: 0 },
                { row: -1, col: 1 },
                { row: -1, col: -1 },
                { row: 0, col: 1 },
                { row: 0, col: -1 }
            ]);
        default:
            return [];
    }
}



export default getValidMoves;