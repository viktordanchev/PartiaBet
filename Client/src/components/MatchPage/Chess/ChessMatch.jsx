import React from 'react';
import { jwtDecode } from 'jwt-decode';
import Board from './Board';
import PlayerCard from '../PlayerCard';

const ChessMatch = ({ data }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const loggedPlayer = data.players.find(p => p.id === userId);
    const opponent = data.players.find(p => p.id !== userId);

    return (
        <section className="flex gap-3">
            <Board />
            <article className="flex flex-col justify-between text-gray-300">
                <div className="space-y-3">
                    <PlayerCard data={opponent} />
                    <article className="h-fit w-fit bg-gray-300 px-2 text-2xl text-center font-semibold rounded shadow-xl shadow-gray-900">
                        <p className="text-gray-800">10:33</p>
                    </article>
                </div>
                <div className="space-y-3">
                    <PlayerCard data={loggedPlayer} />
                    <article className="h-fit w-fit bg-gray-300 px-2 text-2xl text-center font-semibold rounded shadow-xl shadow-gray-900">
                        <p className="text-gray-800">10:33</p>
                    </article>
                </div>
            </article>
        </section>
    );
};

export default ChessMatch;