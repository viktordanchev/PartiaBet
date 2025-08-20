import React from 'react';

const TopPlayers = () => {
    return (
        <article className="bg-gray-800 p-6 rounded-xl border border-gray-500 shadow-xl shadow-gray-900">
            <h2 className="text-2xl font-bold text-gray-200 mb-6 text-center">
                Top 3 Players
            </h2>
            <div className="flex justify-center items-end gap-4">
                <div className="bg-gray-700 p-4 rounded-lg text-gray-300 flex flex-col items-center shadow-lg transform translate-y-4">
                    <span className="text-xl">🥈 Hikaru Nakamura</span>
                    <span>Rating: 2780</span>
                </div>
                <div className="bg-gray-700 p-4 rounded-lg text-gray-300 flex flex-col items-center shadow-lg z-10">
                    <span className="text-xl">🥇 Magnus Carlsen</span>
                    <span>Rating: 2850</span>
                </div>
                <div className="bg-gray-700 p-4 rounded-lg text-gray-300 flex flex-col items-center shadow-lg transform translate-y-8">
                    <span className="text-xl">🥉 Ian Nepomniachtchi</span>
                    <span>Rating: 2765</span>
                </div>
            </div>
        </article>
    );
};

export default TopPlayers;