import React from 'react';

const PlayerStats = ({ playerData }) => {
    const totalWins = playerData.gamesStats.reduce((sum, g) => sum + g.winCount, 0);
    const totalLosses = playerData.gamesStats.reduce((sum, g) => sum + g.lossCount, 0);
    const totalGames = totalWins + totalLosses;

    return (
        <article className="flex flex-col gap-6 bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900">

            <h2 className="text-2xl text-gray-300 font-semibold">
                Overall Stats
            </h2>

            <div className="grid grid-cols-3 text-center text-gray-300">

                <div>
                    <p className="text-gray-400">Wins</p>
                    <p className="text-green-400 text-2xl font-semibold">{totalWins}</p>
                </div>

                <div>
                    <p className="text-gray-400">Losses</p>
                    <p className="text-red-400 text-2xl font-semibold">{totalLosses}</p>
                </div>

                <div>
                    <p className="text-gray-400">Total Games</p>
                    <p className="text-indigo-400 text-2xl font-semibold">{totalGames}</p>
                </div>

            </div>

        </article>
    );
};

export default PlayerStats;