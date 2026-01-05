import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useLoading } from '../../contexts/LoadingContext';
import { useHub } from '../../contexts/HubContext';

function StatusOutMatch() {
    const navigate = useNavigate();
    const { setIsLoading } = useLoading();
    const { connection } = useHub();

    const joinMatch = async () => {
        const matchId = localStorage.getItem('currentMatchId');

        setIsLoading(true);
        await connection.invoke("RejoinMatch", matchId);
        setIsLoading(false);

        navigate(`/games/chess/match/${matchId}`);
    };

    return (
        <button className="px-3 py-1 rounded-xl bg-green-600 shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
            onClick={joinMatch}>
            Rejoin
        </button>
    );
}

export default StatusOutMatch;