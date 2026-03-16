import React from 'react';

const PlayerGames = ({ games }) => {
    return (
        <article className="bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900">

            <h2 className="text-2xl text-gray-300 font-semibold mb-6">
                Game Statistics
            </h2>

            <table className="w-full text-gray-300">

                <thead className="text-gray-400 border-b border-gray-600">
                    <tr>
                        <th className="py-2 text-left">Game</th>
                        <th>Wins</th>
                        <th>Losses</th>
                        <th>Total</th>
                        <th>Rating</th>
                    </tr>
                </thead>

                <tbody>

                    {games.map(game => {

                        const total = game.wins + game.losses;

                        return (
                            <tr key={game.name} className="border-b border-gray-700 text-center">

                                <td className="py-3 text-left">{game.name}</td>
                                <td className="text-green-400">{game.wins}</td>
                                <td className="text-red-400">{game.losses}</td>
                                <td>{total}</td>
                                <td className="text-indigo-400">{game.rating}</td>

                            </tr>
                        );
                    })}

                </tbody>

            </table>

        </article>
    );
};

export default PlayerGames;