import React from 'react';
import PlayButton from './PlayButton';
import FriendChallengeButton from './FriendChallengeButton';

const GameInfo = ({ gameName, gameImg }) => {
    return (
        <article className="flex flex-col gap-4 items-center bg-gray-800 p-6 rounded-xl border border-gray-500 shadow-xl shadow-gray-900">
            <div>
                <img className="h-40 w-40 rounded-xl border border-gray-500 shadow-lg shadow-gray-900"
                    src={gameImg}
                />
                <p className="text-gray-300 text-4xl font-semibold text-center">
                    {gameName}
                </p>
            </div>

            <div className="flex justify-center gap-4">
                <PlayButton />
                <FriendChallengeButton />
            </div>
        </article>
    );
};

export default GameInfo;