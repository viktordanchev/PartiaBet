import React from 'react';
import { useNavigate } from 'react-router-dom';
import PlayerCard from './PlayerCard';
import { useMatchHub } from '../../../contexts/MatchHubContext';
import { useLoading } from '../../../contexts/LoadingContext';

const LobbyList = ({ match }) => {
    const navigate = useNavigate();
    const { connection } = useMatchHub();
    const { setIsLoading } = useLoading();

    const handleCancel = async () => {
        setIsLoading(true);
        await connection.invoke("LeaveMatch", match.id);
        setIsLoading(false);

        navigate('/');
    };

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
                onClick={handleCancel}>
                Cancel
            </button>
        </div>
    );
};

export default LobbyList;