import React, { useState } from 'react';

const FriendCard = ({ username, isOnline }) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <article
            className={`bg-gray-900 rounded-xl border border-gray-500 shadow-xl shadow-gray-900 p-3 flex items-center gap-2 transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:border-white ${isOpen ? 'h-50 w-100 justify-center items-start' : 'h-25 w-70 hover:scale-105'}`}
            onClick={() => setIsOpen(!isOpen)}>
            <img src=""
                className="w-16 h-16 rounded-full object-cover border-2 border-gray-500" />
            <div className="flex flex-col">
                <h2 className="text-white text-lg font-semibold">{username}</h2>
                <span className={`flex items-center text-sm ${isOnline ? 'text-green-400' : 'text-red-400'}`}>
                    <span className={`w-2 h-2 rounded-full mr-2 ${isOnline ? 'bg-green-400' : 'bg-red-400'}`}></span>
                    {isOnline ? 'Online' : 'Offline'}
                </span>
            </div>
        </article>
    );
};

export default FriendCard;