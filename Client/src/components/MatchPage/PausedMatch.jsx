import React, { useEffect } from 'react';
import { useTimer } from 'react-timer-hook';
import { useAppHub } from '../../contexts/AppHubContext';
import useApiRequest from '../../hooks/useApiRequest';

function PausedMatch() {
    const { matchState } = useAppHub();
    const { leaverData } = matchState;
    const apiRequest = useApiRequest();

    const { seconds, minutes, restart } = useTimer({
        expiryTimestamp: new Date(),
        autoStart: false
    });

    const startTimer = (timeLeftSeconds) => {
        if (!timeLeftSeconds || timeLeftSeconds <= 0) return;

        const expiry = new Date(Date.now() + timeLeftSeconds * 1000);
        restart(expiry, true);
    };

    useEffect(() => {
        if (!leaverData) return;

        startTimer(leaverData.timeLeft);
    }, [leaverData]);

    useEffect(() => {
        const fetchRejoinTime = async () => {
            const response = await apiRequest('matches', 'getMatchCountdown', 'GET', true, false); 
           
            startTimer(response.timeLeftToRejoin);
        };

        fetchRejoinTime();
    }, []);

    return (
        <div className="fixed inset-0 backdrop-blur-sm z-50 flex items-center justify-center">
            <div className="bg-gray-900 p-6 rounded-xl text-center text-white">

                <p className="text-xl font-semibold">
                    Match is paused
                </p>

                <p className="text-gray-400 mt-2">
                    Waiting for players to rejoin...
                </p>

                <p className="mt-4 text-xl font-semibold">
                    {minutes}:{seconds.toString().padStart(2, '0')}
                </p>

            </div>
        </div>
    );
}

export default PausedMatch;