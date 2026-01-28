import React from 'react';
import { jwtDecode } from 'jwt-decode';
import Board from './Board';
import PlayerCard from '../PlayerCard';
import TurnTimer from './TurnTimer';

const ChessMatch = ({ data }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const loggedPlayer = data.players.find(p => p.id === userId);
    const opponent = data.players.find(p => p.id !== userId);
    const playerInTurn = data.players.find(p => p.isMyTurn).id;

    return (
        <section className="flex gap-3">
            <Board data={data.board} />
            <article className="flex flex-col justify-between text-gray-300">
                <div className="space-y-3">
                    <PlayerCard data={opponent} />
                    <TurnTimer timeLeft={opponent.turnTimeLeft} start={playerInTurn === opponent.id} />
                </div>
                <div className="space-y-3">
                    <PlayerCard data={loggedPlayer} />
                    <TurnTimer timeLeft={loggedPlayer.turnTimeLeft} start={playerInTurn === loggedPlayer.id} />
                </div>
            </article>
        </section>
    );
};

export default ChessMatch;