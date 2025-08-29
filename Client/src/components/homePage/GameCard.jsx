import React from 'react';
import { useNavigate } from "react-router-dom";
import { useHub } from '../../contexts/HubContext';

const GameCard = ({ data }) => {
    const navigate = useNavigate();
    const { playersCount } = useHub();
    
    return (
        <li className="w-50 h-75 p-4 text-gray-300 rounded-xl bg-gray-900 border border-gray-500 shadow-xl shadow-gray-900 flex flex-col justify-between transform transition-transform duration-300 hover:scale-105 hover:cursor-pointer hover:border-white"
            onClick={() => navigate(`/games/${data.name.toLowerCase()}`, { state: { gameData: data } })}>
            <div>
                <img src={data.imgUrl} className="rounded-xl" />
                <p className="font-medium text-xl text-center break-all">{data.name}</p>
            </div>
            <div className="flex items-center justify-center space-x-1">
                <span className="inline-block w-3 h-3 rounded-full bg-green-500"></span>
                <span className="text-sm">{playersCount[data.name] || 0} online</span>
            </div>
        </li>
    );
};

export default GameCard;