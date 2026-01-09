import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useTimer } from 'react-timer-hook';
import { useMatchHub } from '../../contexts/MatchHubContext';
import RejoinButton from './RejoinButton';

function Alert() {
    const [showButton, setShowButton] = useState(false);
    const { leaverData, rejoinedPlayer } = useMatchHub();

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
        const timeLeft = localStorage.getItem('rejoinTimeLeft');
        if (!timeLeft) return;

        const expiryDate = new Date(timeLeft);

        if (expiryDate <= new Date()) {
            localStorage.removeItem('rejoinTimeLeft');
            return;
        }

        restart(expiryDate, true);
    }, [restart]);

    useEffect(() => {
        if (!leaverData) return;

        const newTime = new Date();
        newTime.setSeconds(newTime.getSeconds() + leaverData.timeLeft);

        localStorage.setItem('rejoinTimeLeft', newTime.toISOString());

        restart(newTime, true);

        const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
        setShowButton(decodedToken['Id'] === leaverData.playerId);
    }, [leaverData]);

    return (
        <>
            {(minutes > 0 || seconds > 0) &&
                <div className="card-wrapper fixed z-10 top-10 right-10 w-50 h-50 p-1 flex flex-col justify-between rounded">
                    <div className="z-20 h-full w-full p-3 border border-gray-500 rounded bg-gray-900 flex flex-col justify-center text-center text-white font-semibold">
                        <div className="text-lg">
                            Match will end after {minutes}:{seconds.toString().padStart(2, '0')} minutes!
                        </div>
                        {showButton && <RejoinButton />}
                    </div>
                </div>}
        </>
    );
}

export default Alert;