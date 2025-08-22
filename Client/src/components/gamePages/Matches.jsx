import React from 'react';
import MatchList from './Matches/MatchList';

const Matches = () => {
    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 space-y-6">
            <h2 className="text-gray-300 text-3xl font-semibold text-center">
                Tables
            </h2>
            <section className="space-y-6">
                <MatchList isCasualGame={false} />
                <MatchList isCasualGame={true} />
            </section>
        </article>
    );
};

export default Matches;