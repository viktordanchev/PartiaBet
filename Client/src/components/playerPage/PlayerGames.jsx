import React from 'react';

const PlayerGames = ({ games }) => {
    return (
        <article className="bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900">

            <h2 className="text-2xl text-gray-300 font-semibold mb-6">
                Game Statistics
            </h2>

            <table className="w-full text-gray-200 bg-white/5 backdrop-blur-md rounded-xl overflow-hidden">

                <thead className="bg-white/10 text-gray-300 text-sm uppercase">
                    <tr>
                        <th className="py-3 text-left pl-4">Game</th>
                        <th>Wins</th>
                        <th>Losses</th>
                        <th>Total</th>
                        <th>Rating</th>
                    </tr>
                </thead>

                <tbody>
                    {games.map(game => {
                        const total = game.winCount + game.lossCount;

                        return (
                            <tr
                                key={game.gameType}
                                className="border-t border-white/10"
                            >
                                <td className="py-3 pl-4">{game.gameType}</td>
                                <td className="text-green-400 text-center">{game.winCount}</td>
                                <td className="text-red-400 text-center">{game.lossCount}</td>
                                <td className="text-center">{total}</td>
                                <td className="text-indigo-400 text-center">{game.rating}</td>
                            </tr>
                        );
                    })}
                </tbody>

            </table>

        </article>
    );
};

export default PlayerGames;