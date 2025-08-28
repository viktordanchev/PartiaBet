import React from 'react';
import ChessRules from './AllGameRules/ChessRules';

const rulesComponents = {
    chess: ChessRules,
    Backgammon: ChessRules,
    Belote: ChessRules,
    'Sixty-Six': ChessRules
};

const GameRules = ({ game }) => {
    const RulesComponent = rulesComponents[game]

    return (
        <article className="col-span-2 text-gray-300 bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900 flex flex-col gap-6">
            <h2 className="text-3xl font-semibold text-center">
                Game Rules
            </h2>
            <RulesComponent />
        </article>
    );
};

export default GameRules;