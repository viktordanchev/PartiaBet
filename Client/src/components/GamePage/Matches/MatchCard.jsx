import React from 'react';
import PlayerCard from './PlayerCard';

const MatchCard = ({ data }) => {
    const isMatchFull = data.players.length === data.maxPlayers;
    console.log(data);

    const teams = data.players.reduce((acc, player) => {
        if (!acc[player.team]) acc[player.team] = [];
        acc[player.team].push(player);
        return acc;
    }, {});

    return (
        <li className="w-full p-2 flex flex-col items-center gap-3 rounded-xl border border-gray-700 bg-gray-900">
            <div className="w-full flex items-center text-xs text-center">
                {Object.entries(teams).map(([teamNumber, players]) => (
                    <div key={teamNumber} className="flex-1 flex justify-center items-center gap-2">
                        {players.map((player) => (
                            <div key={player.id} className="flex flex-col items-center">
                                <img
                                    src={player.profileImgUrl || ProfilePhoto}
                                    className="rounded-lg border border-gray-500 h-12 w-12"
                                />
                                <p className="font-semibold truncate w-24">{player.username}</p>
                                <p>Rating: {player.rating}</p>
                            </div>
                        ))}
                        <p className="flex-none text-2xl font-semibold mx-2">VS</p>
                    </div>
                ))}
            </div>
            <button className={`py-1 px-3 rounded-xl text-white font-medium transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105 ${isMatchFull ? 'bg-red-600' : 'bg-green-600'}`}>
                {isMatchFull ? (
                    <>
                        <span className="inline-block w-2 h-2 rounded-full bg-white mr-1"></span>
                        <span>Live</span>
                    </>
                ) : (
                    <>
                        {data.betAmount === 0 ? 'Play' : `Bet ${data.betAmount}$`}
                    </>
                )}
            </button>
        </li>
    );
};

export default MatchCard;