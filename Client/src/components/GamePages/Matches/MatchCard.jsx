import React from 'react';
import QuestionMark from '../../../assets/images/question-mark.png';

const MatchCard = ({ isCasualGame, isMatchStarted }) => {
    return (
        <li className="w-full p-2 flex flex-col items-center gap-3 rounded-xl border border-gray-700 bg-gray-900">
            <div className="w-full flex items-center text-sm">
                <div className="flex-1 flex flex-col items-center">
                    <img src="" className="rounded-lg border border-gray-500 h-15 w-15" />
                    <p className="font-semibold">1234567890123456</p>
                    <p className="text-xs">Raiting: 2789</p>
                </div>
                <p className="flex-none text-2xl font-semibold">VS</p>
                <div className="flex-1 flex flex-col items-center">
                    <img src={QuestionMark} className="rounded-lg border border-gray-500 h-15 w-15" />
                    <p className="font-semibold">1234567890123456</p>
                    <p className="text-xs">Raiting: 2789</p>
                </div>
            </div>
            <button className={`py-1 px-3 rounded-xl text-white font-medium border-2 border-gray-500 transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105 ${isMatchStarted ? 'bg-red-600' : 'bg-green-600'}`}>
                {isMatchStarted ? (
                    <>
                        <span className="inline-block w-2 h-2 rounded-full bg-white mr-1"></span>
                        <span>Live</span>
                    </>
                ) : (
                    <>
                        {isCasualGame ? 'Play' : 'Bet $20'}
                    </>
                )}
            </button>
        </li>
    );
};

export default MatchCard;