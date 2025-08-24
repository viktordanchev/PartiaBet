import React from 'react';
import PlayButton from './GameInfo/PlayButton';
import FriendChallengeButton from './GameInfo/FriendChallengeButton';

const GameInfo = ({ gameName, gameImg }) => {
    return (
        <article className="flex flex-col gap-4 items-center bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900">
            <div>
                <img className="h-40 w-40 m-auto rounded-xl border border-gray-500 shadow-lg shadow-gray-900"
                    src={gameImg}
                />
                <p className="text-gray-300 text-4xl text-center font-semibold">
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