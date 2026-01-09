import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useLoading } from '../../contexts/LoadingContext';
import { useMatchHub } from '../../contexts/MatchHubContext';

function RejoinButton() {
    const navigate = useNavigate();
    const { setIsLoading } = useLoading();
    const { connection } = useMatchHub();

    const joinMatch = async () => {
        setIsLoading(true);
        var matchId = await connection.invoke("RejoinMatch");
        setIsLoading(false);

        if (matchId) {
            navigate(`/games/chess/match/${matchId}`);
        }
    };

    return (
        <button className="px-3 py-1 rounded-xl bg-green-600 shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
            onClick={joinMatch}>
            Rejoin
        </button>
    );
}

export default RejoinButton;