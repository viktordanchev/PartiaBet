import React, { useEffect } from 'react';
import { useTimer } from 'react-timer-hook';

const getExpiry = (seconds) => {
    const date = new Date();
    date.setSeconds(date.getSeconds() + seconds);
    return date;
};

const TurnTimer = ({ timeLeft, isActive }) => {
    const { seconds, minutes, restart, pause } = useTimer({
        expiryTimestamp: getExpiry(timeLeft),
        autoStart: false
    });

    useEffect(() => {
        if (isActive) {
            restart(getExpiry(timeLeft), true);
        } else {
            pause();
        }
    }, [isActive, timeLeft]);

    return (
        <article className="h-fit w-fit bg-gray-300 px-2 text-2xl text-center font-semibold rounded shadow-xl shadow-gray-900">
            <p className="text-gray-800">
                {String(minutes).padStart(2, '0')}:
                {String(seconds).padStart(2, '0')}
            </p>
        </article>
    );
};

export default TurnTimer;
