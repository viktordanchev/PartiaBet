import React from 'react';
import { jwtDecode } from 'jwt-decode';

const EndingScreen = ({ winners }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));

    const isWinner = winners.some(w => w.id === decodedToken['Id']);

    const currentPlayer = winners.find(w => w.id === decodedToken['Id']);

    const ratingChange = currentPlayer
        ? currentPlayer.ratingChange
        : -15;

    return (
        <div className="fixed inset-0 bg-black/70 backdrop-blur-sm z-50 flex items-center justify-center">
            <div className="bg-gray-900 w-[500px] max-w-[95%] p-8 rounded-2xl shadow-2xl text-center text-white">

                {/* RESULT */}
                <h1
                    className={`text-5xl font-bold mb-6 ${isWinner ? "text-green-400" : "text-red-400"
                        }`}
                >
                    {isWinner ? "WIN" : "LOSE"}
                </h1>

                {/* RATING CHANGE */}
                <p
                    className={`text-xl mb-6 font-semibold ${isWinner ? "text-green-300" : "text-red-300"
                        }`}
                >
                    {isWinner ? "+" : ""}
                    {ratingChange} Rating
                </p>

                {/* WINNERS LIST */}
                <div>
                    <h2 className="text-lg mb-4 text-gray-300">
                        Winners
                    </h2>

                    <div className="flex flex-col gap-4">
                        {winners.map(player => (
                            <div
                                key={player.id}
                                className="flex items-center gap-4 bg-gray-800 p-3 rounded-lg"
                            >
                                <img
                                    src={player.profileImageUrl}
                                    alt={player.username}
                                    className="w-12 h-12 rounded-full object-cover border-2 border-yellow-400"
                                />

                                <div className="flex-1 text-left">
                                    <p className="font-semibold">
                                        {player.username}
                                    </p>
                                    <p className="text-sm text-gray-400">
                                        +{player.ratingChange} Rating
                                    </p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

            </div>
        </div>
    );
};

export default EndingScreen;