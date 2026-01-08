import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useTimer } from 'react-timer-hook';
import { useMatchHub } from '../../contexts/MatchHubContext';
import StatusOutMatch from './StatusOutMatch';

function Alert() {
    const [showButtons, setShowButtons] = useState(false);
    const { connection } = useMatchHub();

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
        const savedExpiry = localStorage.getItem('rejoinExpiry');
        if (!savedExpiry) return;

        const expiryDate = new Date(savedExpiry);

        if (expiryDate <= new Date()) {
            localStorage.removeItem('rejoinExpiry');
            return;
        }

        restart(expiryDate, true);
    }, [restart]);

    useEffect(() => {
        if (!connection) return;

        const handleTimeLeftToRejoin = (palyerId, secondsLeft) => {
            const newTime = new Date();
            newTime.setSeconds(newTime.getSeconds() + secondsLeft);

            localStorage.setItem('rejoinExpiry', newTime.toISOString());

            restart(newTime, true);

            const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
            setShowButtons(decodedToken['Id'] === palyerId);
        };

        connection.on("TimeLeftToRejoin", handleTimeLeftToRejoin);

        return () => {
            connection.off("TimeLeftToRejoin", handleTimeLeftToRejoin);
        };
    }, [connection]);

    return (
        <>
            {(minutes > 0 || seconds > 0) &&
                <div className="card-wrapper fixed z-10 top-10 right-10 w-50 h-50 p-1 flex flex-col justify-between rounded">
                    <div className="z-20 h-full w-full p-3 border border-gray-500 rounded bg-gray-900 flex flex-col justify-center text-center text-white font-semibold">
                        <div className="text-lg">
                            Match will end after {minutes}:{seconds.toString().padStart(2, '0')} minutes!
                        </div>
                        {showButtons && <StatusOutMatch />}
                    </div>
                </div>}
        </>
    );
}

export default Alert;