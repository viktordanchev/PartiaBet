import React from 'react';
import MatchCard from './Matches/MatchCard';

const Matches = () => {
    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 flex flex-col gap-6">
            <h2 className="text-2xl font-bold text-center">
                Open Tables
            </h2>
            <section className="flex justify-between">
                <article className="w-1/2 flex flex-col gap-6">
                    <h3 className="text-xl text-center">Play with bets</h3>
                    <ul className="grid grid-cols-2 gap-3">
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                    </ul>
                </article>
                <div className="border-l border-gray-500 mx-6"></div>
                <article className="w-1/2 flex flex-col gap-6">
                    <h3 className="text-xl text-center">Play for fun</h3>
                    <ul className="grid grid-cols-2 gap-3">
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                    </ul>
                </article>
            </section>
        </article>
    );
};

export default Matches;