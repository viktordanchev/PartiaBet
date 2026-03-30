import React, { useEffect } from 'react';
import { useMatch } from 'react-router-dom';
import { useTimer } from 'react-timer-hook';
import { useAuth } from '../../contexts/AuthContext';
import RejoinButton from './RejoinButton';
import useApiRequest from '../../hooks/useApiRequest';
import { useMatchHub } from '../../contexts/MatchHubContext';

const COUNTDOWN_KEY = 'matchEndCountdown';

function ActiveMatchAlert() {
    const { leaverData } = useMatchHub();
    const { isAuthenticated } = useAuth();
    const apiRequest = useApiRequest();

    const { seconds, minutes, restart } = useTimer({
        expiryTimestamp: new Date(),
        autoStart: false
    });

    const inMatch = !!useMatch('/games/:game/match/:matchId');

    const startTimer = (timeLeftSeconds) => {
        if (!timeLeftSeconds || timeLeftSeconds <= 0) return;

        const expiry = new Date(Date.now() + timeLeftSeconds * 1000);

        restart(expiry, true);
        localStorage.setItem(COUNTDOWN_KEY, expiry.toISOString());
    };

    useEffect(() => {
        const fetchRejoinTime = async () => {
            const response = await apiRequest('matches', 'getMatchCountdown', 'GET', true, false);

            if (!response || response.timeLeftToRejoin === 0) return;

            sessionStorage.setItem('connection-matchId', response.matchId);

            startTimer(response.timeLeftToRejoin);
        };

        if (isAuthenticated) {
            fetchRejoinTime();
        }
    }, [isAuthenticated]);

    useEffect(() => {
        if (!leaverData) return;
        console.log('Received leaverData:', leaverData);
        startTimer(leaverData.timeLeft);
    }, [leaverData]);

    useEffect(() => {
        const stored = localStorage.getItem(COUNTDOWN_KEY);
        if (!stored) return;

        const expiry = new Date(stored);

        if (expiry <= new Date()) {
            localStorage.removeItem(COUNTDOWN_KEY);
            return;
        }

        restart(expiry, true);
    }, [restart]);

    if ((minutes === 0 && seconds === 0) || inMatch) return null;

    return (
        <div className="card-wrapper relative z-10 w-50 h-50 p-1 flex flex-col justify-between rounded-xl">
            <div className="z-20 h-full w-full p-3 border border-gray-500 rounded-xl bg-gray-900 flex flex-col text-center text-white font-semibold">
                <div className="flex-1 flex items-center justify-center text-lg">
                    Match will end after {minutes}:{seconds.toString().padStart(2, '0')} minutes!
                </div>

                <RejoinButton
                    onRejoin={() => {
                        restart(new Date(), false);
                        localStorage.removeItem(COUNTDOWN_KEY);
                    }}
                />
            </div>
        </div>
    );
}

export default ActiveMatchAlert;