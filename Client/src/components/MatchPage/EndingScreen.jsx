import React, { useEffect, useState } from 'react';

const EndingScreen = () => {

    const isWinner = false;

    const oldRating = 1000;
    const ratingChange = isWinner ? 30 : -30;
    const newRating = oldRating + ratingChange;

    const [displayRating, setDisplayRating] = useState(oldRating);

    useEffect(() => {

        let start = oldRating;
        const end = newRating;

        const step = start < end ? 1 : -1;

        const interval = setInterval(() => {

            start += step;
            setDisplayRating(start);

            if (start === end) {
                clearInterval(interval);
            }

        }, 70);

        return () => clearInterval(interval);

    }, []);

    const winners = [
        {
            id: "1",
            username: "viktordanchev12",
            profileImageUrl: "https://i.pravatar.cc/100?img=1",
            ratingChange: 30,
            newRating: 1030
        },
        {
            id: "2",
            username: "playerTwo",
            profileImageUrl: "https://i.pravatar.cc/100?img=2",
            ratingChange: 25,
            newRating: 1015
        },
        {
            id: "3",
            username: "playerThree",
            profileImageUrl: "https://i.pravatar.cc/100?img=3",
            ratingChange: 18,
            newRating: 1008
        }
    ];

    return (
        <div className="fixed inset-0 backdrop-blur-sm z-50 flex items-center justify-center">

            <div className={`bg-gray-900 w-[500px] max-w-[95%] p-8 rounded-xl shadow-2xl text-center text-white border-2 ${isWinner ? "border-green-400" : "border-red-400"}`}>

                {/* Result */}
                <h1 className={`text-5xl font-bold ${isWinner ? "text-green-400" : "text-red-400"}`}>
                    {isWinner ? "WIN" : "LOSE"}
                </h1>

                {/* Rating Section */}
                <div className="my-6">

                    <p className="text-gray-400 text-sm uppercase tracking-widest">
                        New Rating
                    </p>

                    <p className="text-4xl font-bold">
                        {displayRating}
                    </p>

                    <p className={`text-lg font-semibold ${ratingChange > 0 ? "text-green-400" : "text-red-400"}`}>
                        {ratingChange > 0 ? "+" : ""}{ratingChange}
                    </p>

                </div>

                {/* Winners */}
                <div>

                    <h2 className="text-lg mb-3 text-gray-300">
                        Winners
                    </h2>

                    <div className="flex flex-col gap-3">

                        {winners.map(player => (
                            <div
                                key={player.id}
                                className="flex items-center gap-4 bg-gray-800 p-3 rounded-xl"
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
                                        New {player.newRating}
                                        <span className={`ml-2 font-semibold ${player.ratingChange > 0 ? "text-green-400" : "text-red-400"}`}>
                                            {player.ratingChange > 0 ? "+" : ""}{player.ratingChange}
                                        </span>
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