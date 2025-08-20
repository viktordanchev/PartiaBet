import React, { useState, useRef } from 'react';
import { useClickOutside } from '../../hooks/useClickOutside';

const FriendChallengeButton = () => {
    const [showFriends, setShowFriends] = useState(false);
    const dropdownRef = useRef(null);

    const friends = ["Alice", "Bob", "Charlie", "Diana", "1234567890123456"];

    useClickOutside(dropdownRef, () => setShowFriends(false));

    return (
        <div className={`relative px-4 py-2 rounded-xl bg-blue-600 text-white text-lg font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer ${showFriends ? "scale-105" : "hover:scale-105"
            }`}
            onClick={() => setShowFriends(!showFriends)}
        >
            Challenge a Friend

            {showFriends && (
                <div className="absolute top-full left-1/2 -translate-x-1/2 mt-2 w-60 h-60 p-3 border border-gray-500 shadow-xl shadow-gray-900 bg-gray-900 rounded-xl flex flex-col gap-3 z-10"
                    ref={dropdownRef}
                    onClick={(e) => e.stopPropagation()}
                >
                    <div className="overflow-x-auto scrollbar-sidebar flex flex-col gap-2">
                        {friends.map((friend, index) => (
                            <button
                                key={index}
                                className="w-full py-1 rounded-xl bg-gray-800 text-gray-300 font-medium transition-all duration-200 hover:bg-maincolor hover:text-gray-800 hover:cursor-pointer"
                                onClick={() => {
                                    setShowFriends(false);
                                }}
                            >
                                {friend}
                            </button>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
};

export default FriendChallengeButton;
