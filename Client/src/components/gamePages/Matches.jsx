import React from 'react';
import MatchCard from './Matches/MatchCard';

const Matches = () => {
    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 flex flex-col gap-6">
            <h2 className="text-2xl font-bold text-center">
                Open Tables
            </h2>
            <section className="flex flex-col">
                <article className="flex flex-col gap-6">
                    <h3 className="text-xl text-center">Play with bets</h3>
                    <ul className="grid grid-cols-3 gap-3">
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <div className="m-auto">
                            <button className="py-2 px-6 rounded-xl bg-blue-600 text-lg text-white font-semibold shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                                Load more
                            </button>
                        </div>
                    </ul>
                </article>
                <div className="border-t border-gray-500 my-6"></div>
                <article className="flex flex-col gap-6">
                    <h3 className="text-xl text-center">Play for fun</h3>
                    <ul className="grid grid-cols-3 gap-3">
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <MatchCard />
                        <div className="m-auto">
                            <button className="py-2 px-6 rounded-xl bg-blue-600 text-lg text-white font-semibold shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                                Load more
                            </button>
                        </div>
                    </ul>
                </article>
            </section>
        </article>
    );
};

export default Matches;