import React, { useEffect } from 'react';
import { useTimer } from 'react-timer-hook';
import { useMatchHub } from '../../../contexts/MatchHubContext';

const getExpiry = (seconds) => {
    const date = new Date();
    date.setSeconds(date.getSeconds() + seconds);
    return date;
};

const TurnTumer = ({ timeLeft, start }) => {
    const { seconds, minutes, restart } = useTimer({
        expiryTimestamp: getExpiry(timeLeft),
        autoStart: start
    });
    const { newMove } = useMatchHub();

    useEffect(() => {
        if (!newMove) return;

        restart(getExpiry(newMove.duration), true);
    }, [newMove]);

    return (
        <article className="h-fit w-fit bg-gray-300 px-2 text-2xl text-center font-semibold rounded shadow-xl shadow-gray-900">
            <p className="text-gray-800">
                {String(minutes).padStart(2, '0')}:
                {String(seconds).padStart(2, '0')}
            </p>
        </article>
    );
};

export default TurnTumer;