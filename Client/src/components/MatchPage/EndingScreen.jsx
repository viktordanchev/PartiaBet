import React, { useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import { jwtDecode } from 'jwt-decode';

const EndingScreen = ({ players }) => {
    const navigate = useNavigate();
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const myStats = players.find(p => p.id === userId);
    const otherPlayers = players.filter(p => p.id !== userId);
    const isWinner = myStats.isWinner;
    const [displayRating, setDisplayRating] = useState(myStats?.currentRating ?? 0);
    const ratingDiff = myStats.newRating - myStats.currentRating;
    
    useEffect(() => {
        let start = myStats.currentRating;
        const end = myStats.newRating;
       
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

    return (
        <div className="fixed inset-0 backdrop-blur-sm z-50 flex items-center justify-center">

            <div className={`bg-gray-900 w-[500px] max-w-[95%] p-8 rounded-xl shadow-2xl text-center text-white border-2 ${isWinner ? "border-green-400" : "border-red-400"}`}>

                <h1 className={`text-5xl font-bold ${isWinner ? "text-green-400" : "text-red-400"}`}>
                    {isWinner ? "WIN" : "LOSE"}
                </h1>

                <div className="my-6">

                    <p className="text-gray-400 text-sm uppercase tracking-widest">
                        Your New Rating
                    </p>

                    <p className="text-4xl font-bold">
                        {displayRating}
                    </p>

                    <p className={`text-lg font-semibold ${ratingDiff > 0 ? "text-green-400" : "text-red-400"}`}>
                        {ratingDiff > 0 && "+"}{ratingDiff}
                    </p>

                </div>

                <div>

                    <h2 className="text-lg mb-3 text-gray-300">
                        Others
                    </h2>

                    <div className="flex flex-col gap-3">
                        {otherPlayers.map(player => {

                            const diff = player.newRating - player.currentRating;

                            return (
                                <div key={player.id}
                                    className="flex items-center gap-4 bg-gray-800 p-3 rounded-xl">

                                    <img src={player.profileImageUrl}
                                        alt={player.username}
                                        className="w-12 h-12 rounded-full object-cover border-2 border-maincolor" />

                                    <div className="flex-1 text-left">

                                        <p className="font-semibold">{player.username}</p>

                                        <p className="text-sm text-gray-400">
                                            New Rating: {player.newRating}
                                            <span className={`ml-4 font-semibold ${diff > 0 ? "text-green-400" : "text-red-400"}`}>
                                                {diff > 0 && "+"}{diff}
                                            </span>
                                        </p>

                                    </div>

                                </div>
                            );

                        })}
                    </div>

                </div>

                <button className="mt-8 bg-maincolor text-gray-900 text-xl font-medium py-2 px-8 rounded-xl hover:bg-[#81e4dc] hover:cursor-pointer"
                    onClick={() => navigate("/")}>
                    Exit
                </button>

            </div>

        </div>
    );
};

export default EndingScreen;