import React from 'react';
import QuestionMark from '../../../assets/images/profile-photo.jpg';

const MatchCard = () => {
    return (
        <li className="w-full p-2 flex flex-col items-center gap-3 rounded-xl shadow-md shadow-gray-900 border border-gray-700 bg-gray-900">
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
            <button className="py-1 px-3 rounded-xl bg-green-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                Bet $20
            </button>
        </li>
    );
};

export default MatchCard;