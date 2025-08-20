import React from 'react';
import { useNavigate } from "react-router-dom";

const GameCard = ({ gameName, gameImg, playersCount }) => {
    const navigate = useNavigate();

    return (
        <li className="w-50 h-75 p-4 text-gray-300 rounded-xl bg-gray-900 border border-gray-500 shadow-xl shadow-gray-900 flex flex-col justify-between transform transition-transform duration-300 hover:scale-105 hover:cursor-pointer hover:border-white"
            onClick={() => navigate(`/games/${gameName.toLowerCase()}`)}>
            <div>
                <img src={gameImg} className="rounded-xl" />
                <p className="font-medium text-xl text-center break-all">{gameName}</p>
            </div>
            <div className="flex items-center justify-center space-x-1">
                <span className="inline-block w-3 h-3 rounded-full bg-green-500"></span>
                <span className="text-sm">{playersCount} online</span>
            </div>
        </li>
    );
};

export default GameCard;