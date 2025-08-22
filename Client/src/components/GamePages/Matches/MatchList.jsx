import React from 'react';
import MatchCard from './MatchCard';

const MatchList = ({ isCasualGame }) => {
    return (
        <article className="space-y-3">
            <h3 className="text-2xl text-center">{isCasualGame ? 'Casual Play' : 'Play with bets'}</h3>
            <ul className="grid grid-cols-3 gap-3 p-3 rounded [box-shadow:inset_0_0_30px_rgba(81,218,207,0.3)]">
                <MatchCard
                    isCasualGame={isCasualGame}
                    isMatchStarted={true}
                />
                <MatchCard
                    isCasualGame={isCasualGame}
                    isMatchStarted={false}
                />
                <MatchCard
                    isCasualGame={isCasualGame}
                    isMatchStarted={false}
                />
                <div className="col-span-3 m-auto">
                    <button className="py-2 px-6 rounded-xl bg-blue-600 text-lg text-white font-semibold shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                        Load more
                    </button>
                </div>
            </ul>
        </article>
    );
};

export default MatchList;