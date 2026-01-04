import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { useTimer } from 'react-timer-hook';
import { useHub } from '../contexts/HubContext';
import { useLoading } from '../contexts/LoadingContext';

function ActiveMatchNotification() {
    const [showButtons, setShowButtons] = useState(false);
    const navigate = useNavigate();
    const { setIsLoading } = useLoading();
    const { connection } = useHub();

    const time = new Date();
    time.setSeconds(time.getSeconds() + 0);

    const {
        seconds,
        minutes,
        restart,
    } = useTimer({
        expiryTimestamp: time
    });

    useEffect(() => {
        if (!connection) return;

        const handleTimeLeftToRejoin = (palyerId, secondsLeft) => {
            const newTime = new Date();
            newTime.setSeconds(newTime.getSeconds() + secondsLeft);
            restart(newTime, true);

            const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
            setShowButtons(decodedToken['Id'] === palyerId);
        };

        connection.on("TimeLeftToRejoin", handleTimeLeftToRejoin);

        return () => {
            connection.off("TimeLeftToRejoin", handleTimeLeftToRejoin);
        };
    }, [connection, restart]);

    const joinMatch = async () => {
        const matchId = localStorage.getItem('currentMatchId');
        const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
        var playerData = {
            id: decodedToken['Id'],
            username: decodedToken['Username'],
            profileImageUrl: decodedToken['ProfileImageUrl'],
        };

        setIsLoading(true);
        await connection.invoke("JoinMatch", matchId, playerData);
        setIsLoading(false);

        navigate(`/games/chess/match/${matchId}`);
    };

    return (
        <>
            {(minutes > 0 || seconds > 0) &&
                <div className="card-wrapper fixed z-10 top-10 right-10 w-50 h-50 p-1 flex flex-col justify-between rounded">
                    <div className="z-20 h-full w-full p-3 border border-gray-500 rounded bg-gray-900 flex flex-col justify-between">
                        <div className="text-white font-semibold text-lg mb-2">
                            You have {minutes}:{seconds.toString().padStart(2, '0')} minutes to rejoin your match!
                        </div>
                        {showButtons &&
                            <div className="flex justify-between text-white font-medium">
                                <button className="px-3 py-1 rounded-xl bg-green-600 shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                                    onClick={joinMatch}>
                                    Join
                                </button>
                                <button className="px-3 py-1 rounded-xl bg-red-600 shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                                    Cancel
                                </button>
                            </div>}
                    </div>
                </div>}
        </>
    );
}

export default ActiveMatchNotification;