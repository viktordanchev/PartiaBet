import React from 'react';
import MatchList from './Matches/MatchList';

const Matches = () => {
    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 space-y-3">
            <h2 className="text-gray-300 text-3xl font-semibold text-center">
                Tables
            </h2>
            <section className="flex flex-col">
                <MatchList isCasualGame={false} />
                <div className="border-t border-gray-500 my-6"></div>
                <MatchList isCasualGame={true} />
            </section>
        </article>
    );
};

export default Matches;