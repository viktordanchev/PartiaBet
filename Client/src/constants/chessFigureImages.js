// src/constants/figureImages.js

// Импортирай всички SVG-и
import BlackKing from '../assets/images/chess/figures/black-king.svg';
import BlackQueen from '../assets/images/chess/figures/black-queen.svg';
import BlackRook from '../assets/images/chess/figures/black-rook.svg';
import BlackBishop from '../assets/images/chess/figures/black-bishop.svg';
import BlackKnight from '../assets/images/chess/figures/black-knight.svg';
import BlackPawn from '../assets/images/chess/figures/black-pawn.svg';

import WhiteKing from '../assets/images/chess/figures/white-king.svg';
import WhiteQueen from '../assets/images/chess/figures/white-queen.svg';
import WhiteRook from '../assets/images/chess/figures/white-rook.svg';
import WhiteBishop from '../assets/images/chess/figures/white-bishop.svg';
import WhiteKnight from '../assets/images/chess/figures/white-knight.svg';
import WhitePawn from '../assets/images/chess/figures/white-pawn.svg';

const figureImages = {
    King: { white: WhiteKing, black: BlackKing },
    Queen: { white: WhiteQueen, black: BlackQueen },
    Rook: { white: WhiteRook, black: BlackRook },
    Bishop: { white: WhiteBishop, black: BlackBishop },
    Knight: { white: WhiteKnight, black: BlackKnight },
    Pawn: { white: WhitePawn, black: BlackPawn }
};

const getFigureImage = (pieceType, isWhite) => {
    const color = isWhite ? 'white' : 'black';
    return figureImages[pieceType]?.[color] ?? null;
};

export default getFigureImage;
