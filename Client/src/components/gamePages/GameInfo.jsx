import React from 'react';
import PlayButton from './PlayButton';
import FriendChallengeButton from './FriendChallengeButton';

const GameInfo = ({ gameName, gameImg }) => {
    return (
        <article className="flex flex-col items-center bg-gray-800 p-6 rounded-xl border border-gray-500 shadow-xl shadow-gray-900">
            <img className="h-40 w-40 rounded-xl border border-gray-500 shadow-lg shadow-gray-900"
                src={gameImg}
            />
            <p className="text-gray-200 text-4xl font-semibold text-center mt-4">
                {gameName}
            </p>

            <div className="flex flex-wrap justify-center gap-4 mt-4">
                <PlayButton />
                <FriendChallengeButton />
            </div>
        </article>
    );
};

export default GameInfo;