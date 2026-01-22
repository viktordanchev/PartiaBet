import React from 'react';
import { useNavigate } from 'react-router-dom';
import PlayerCard from './PlayerCard';

const LobbyList = ({ match }) => {
    const navigate = useNavigate();

    return (
        <div className="fixed inset-0 z-50 flex flex-col gap-6 items-center justify-center bg-black/70 text-white">
            <p className="text-2xl font-semibold">
                Waiting...
            </p>
            <div className="flex gap-6">
                {match.players.map((player) =>
                    <PlayerCard
                        key={player.id}
                        data={player}
                    />
                )}
                {Array.from({ length: match.maxPlayersCount - match.players.length }).map((_, index) => (
                    <PlayerCard
                        key={index}
                        data={null}
                    />
                ))}
            </div>
            <button className="p-2 rounded-xl bg-red-600 text-xl font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                onClick={() => navigate('/')}>
                Cancel
            </button>
        </div>
    );
};

export default LobbyList;