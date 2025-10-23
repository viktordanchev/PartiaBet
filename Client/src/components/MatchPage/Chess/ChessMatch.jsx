import React from 'react';
import { jwtDecode } from 'jwt-decode';
import Board from './Board';
import PlayerCard from './PlayerCard';

const ChessMatch = ({ data }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const loggedPlayer = data.players.find(p => p.id === userId);
    const opponent = data.players.find(p => p.id !== userId);

    return (
        <section className="flex gap-3">
            <Board />
            <article className="flex flex-col justify-between text-gray-300">
                <PlayerCard data={opponent} />
                <PlayerCard data={loggedPlayer} />
            </article>
        </section>
    );
};

export default ChessMatch;