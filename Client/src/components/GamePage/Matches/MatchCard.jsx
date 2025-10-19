import React from 'react';
import PlayerCard from './PlayerCard';

const MatchCard = ({ data }) => {
    const isMatchFull = false;
    
    return (
        <li className="w-full p-2 flex flex-col items-center gap-3 rounded-xl border border-gray-700 bg-gray-900">
            <div className="w-full flex flex-col text-center gap-1">
                <p>{data.players.length}/{data.maxPlayersCount}</p>
                <div className="flex justify-evenly text-xs">
                    {data.players.map((player) =>
                        <PlayerCard
                            key={player.id}
                            data={player}
                        />
                    )}
                    {Array.from({ length: data.maxPlayersCount - data.players.length }).map((_, index) => (
                        <PlayerCard
                            key={index}
                            data={null}
                        />
                    ))}
                </div>
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