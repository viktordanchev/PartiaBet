import React from 'react';
import ChessImg from '../assets/images/chess.jpg';
import TopPlayers from '../components/GamePages/TopPlayers';
import GameInfo from '../components/GamePages/GameInfo';
import Matches from '../components/GamePages/Matches';

function ChessGamePage() {
    return (
        <section className="flex-grow grid grid-cols-2 gap-6">
            <GameInfo
                gameName={'Chess'}
                gameImg={ChessImg}
            />
            <TopPlayers />
            <Matches />

            <article className="col-span-2 bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900 flex flex-col gap-3">
                <h2 className="text-2xl font-bold text-gray-300 mb-2 text-center">
                    How to Play Chess
                </h2>
                <p className="text-gray-400">
                    Chess is a two-player strategy board game played on an 8x8 grid. The objective is to checkmate your opponent's king, placing it under direct attack with no legal move to escape.
                </p>
                <h3 className="text-xl font-semibold text-gray-200 mt-4">Piece Movements:</h3>
                <ul className="list-disc list-inside text-gray-400 space-y-2">
                    <li><strong>King:</strong> Moves one square in any direction. The most crucial piece; if checkmated, you lose.</li>
                    <li><strong>Queen:</strong> Moves any number of squares in any direction—vertically, horizontally, or diagonally.</li>
                    <li><strong>Rook:</strong> Moves any number of squares along a row or column.</li>
                    <li><strong>Bishop:</strong> Moves diagonally any number of squares.</li>
                    <li><strong>Knight:</strong> Moves in an "L" shape: two squares in one direction and then one square perpendicular, or one square in one direction and then two squares perpendicular. Knights can jump over other pieces.</li>
                    <li><strong>Pawn:</strong> Moves forward one square but captures diagonally. On its first move, it can move forward two squares. Upon reaching the opposite end of the board, a pawn can be promoted to any other piece (except a king).</li>
                </ul>
                <p className="text-gray-400 mt-4">
                    Special moves include <strong>castling</strong> (a move involving the king and a rook) and <strong>en passant</strong> (a special pawn capture).
                </p>
                <p className="text-gray-400 mt-4">
                    A game ends in checkmate, stalemate, or draw. The player with the white pieces always moves first.
                </p>
            </article>
        </section>
    );
}

export default ChessGamePage;