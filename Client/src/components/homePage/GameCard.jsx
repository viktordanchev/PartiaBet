import React from 'react';

const GameCard = ({ gameName, gameImg, playersCount }) => {
    return (
        <li className="w-44 h-66 p-3 rounded-xl bg-gray-900 border border-maincolor shadow-xl shadow-gray-900 flex flex-col justify-between transform transition-transform duration-300 hover:scale-105 hover:cursor-pointer">
            <div>
                <img src={gameImg} className="rounded-xl" />
                <p className="text-white font-medium text-xl text-center break-all">{gameName}</p>
            </div>
            <div className="flex items-center justify-center space-x-1">
                <span className="inline-block w-3 h-3 rounded-full bg-green-500"></span>
                <span className="text-sm text-gray-300">{playersCount} online</span>
            </div>
        </li>
    );
};

export default GameCard;