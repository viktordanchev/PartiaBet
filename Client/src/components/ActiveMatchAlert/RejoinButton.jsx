import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useLoading } from '../../contexts/LoadingContext';
import { useMatchHub } from '../../contexts/MatchHubContext';

function RejoinButton({ onRejoin }) {
    const navigate = useNavigate();
    const { setIsLoading } = useLoading();
    const { ensureConnection } = useMatchHub();

    const rejoinMatch = async () => {
        const matchId = sessionStorage.getItem('connection-matchId');

        setIsLoading(true);
        const connection = await ensureConnection();
        await connection.invoke("JoinMatchGroup", matchId);
        await connection.invoke("RejoinMatch");
        setIsLoading(false);

        onRejoin();
        localStorage.removeItem('matchEndCountdown');

        if (matchId) {
            navigate(`/games/chess/match/${matchId}`);
        }
    };

    return (
        <button className="px-3 py-1 rounded-xl bg-green-600 shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
            onClick={rejoinMatch}>
            Rejoin
        </button>
    );
}

export default RejoinButton;