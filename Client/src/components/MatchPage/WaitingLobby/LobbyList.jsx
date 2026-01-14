import React from 'react';
import PlayerCard from './PlayerCard';

const LobbyList = ({ matchData }) => {
    return (
        <div className="fixed inset-0 z-50 flex flex-col gap-6 items-center justify-center bg-black/70">
            <p>{matchData.players.length}/{matchData.maxPlayersCount}</p>
            <div className="flex gap-6">
                {matchData.players.map((player, index) =>
                    <PlayerCard
                        key={index}
                        data={player}
                    />
                )}
                {Array.from({ length: matchData.maxPlayersCount - matchData.players.length }).map((_, index) => (
                    <PlayerCard
                        key={index}
                        data={null}
                    />
                ))}
            </div>
            <button className="p-2 rounded-xl bg-red-600 text-xl text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                Cancel
            </button>
        </div>
    );
};

export default LobbyList;