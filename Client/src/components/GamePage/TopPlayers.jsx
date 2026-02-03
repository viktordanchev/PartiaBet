import React from 'react';

const TopPlayers = () => {
    return (
        <article className="bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900 text-gray-300 flex flex-col space-y-6">
            <h2 className="text-3xl font-semibold text-center">
                Top 3 Players
            </h2>
            <div className="h-full flex justify-center items-end gap-4 text-xs">
                <div className="h-5/6 bg-gray-500/40 p-4 rounded-t-lg text-center flex flex-col items-center">
                    <img src="" className="h-10 w-10 rounded-lg border border-gray-500" />
                    <span className="font-semibold">1234567890123456</span>
                    <span>Rating: 2756</span>
                </div>
                <div className="h-full bg-yellow-300/40 p-4 rounded-t-lg text-center flex flex-col items-center">
                    <img src="" className="h-10 w-10 rounded-lg border border-gray-500" />
                    <span className="font-semibold">1234567890123456</span>
                    <span>Rating: 2756</span>
                </div>
                <div className="h-4/6 bg-yellow-900/40 p-4 rounded-t-lg text-center flex flex-col items-center">
                    <img src="" className="h-10 w-10 rounded-lg border border-gray-500" />
                    <span className="font-semibold">12345678901234</span>
                    <span>Rating: 2756</span>
                </div>
            </div>
        </article>
    );
};

export default TopPlayers;