import React from 'react';
import { useNavigate } from "react-router-dom";

const GameCard = ({ data }) => {
    const navigate = useNavigate();
    
    return (
        <article className="w-50 h-75 p-4 text-gray-300 rounded-xl bg-gray-900 border border-gray-500 shadow-xl shadow-gray-900 transform transition-transform duration-300 hover:scale-105 hover:cursor-pointer hover:border-white"
            onClick={() => navigate(`/games/${data.name.toLowerCase()}`)}>
            <img src={data.imageUrl} className="rounded-xl" />
            <p className="font-medium text-xl text-center break-all">{data.name}</p>
        </article>
    );
};

export default GameCard;