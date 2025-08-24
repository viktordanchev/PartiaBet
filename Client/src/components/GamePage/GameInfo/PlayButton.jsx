import React, { useState, useRef } from 'react';
import { useClickOutside } from '../../../hooks/useClickOutside';

function PlayButton() {
    const [showStakeOptions, setShowStakeOptions] = useState(false);
    const [selectedStake, setSelectedStake] = useState(null);
    const [customAmount, setCustomAmount] = useState('');
    const dropdownRef = useRef(null);

    const stakeAmounts = [5, 10, 20, 50, 100];

    useClickOutside(dropdownRef, () => setShowStakeOptions(false));

    return (
        <div className={`relative px-4 py-2 rounded-xl bg-green-600 text-white text-lg font-medium transform transition-all duration-300 ease-in-out hover:cursor-pointer ${showStakeOptions ? "scale-105" : "hover:scale-105"
            }`}
            onClick={() => setShowStakeOptions(!showStakeOptions)}
        >
            Play

            {showStakeOptions && (
                <div className="absolute top-full left-1/2 -translate-x-1/2 mt-2 w-80 p-3 border border-gray-500 shadow-xl shadow-gray-900 bg-gray-900 rounded-xl flex flex-col gap-3 z-10"
                    ref={dropdownRef}
                    onClick={(e) => e.stopPropagation()}
                >
                    <div className="flex flex-col gap-3">
                        <div className="flex justify-between gap-2">
                            {stakeAmounts.map((amount) => (
                                <button
                                    key={amount}
                                    className={`flex-1 py-1 rounded-xl text-gray-300 font-medium border border-gray-500 transition-all duration-200 hover:scale-105 hover:cursor-pointer ${selectedStake === amount ? "bg-maincolor text-gray-800" : "bg-gray-800 hover:bg-maincolor hover:text-gray-800"}`}
                                    onClick={() => {
                                        setSelectedStake(amount);
                                        setCustomAmount("");
                                    }}
                                >
                                    ${amount}
                                </button>
                            ))}
                        </div>

                        <input className="w-full px-3 py-2 rounded-xl bg-gray-800 border border-gray-500 text-gray-300 transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-maincolor"
                            type="number"
                            placeholder="Enter amount"
                            value={customAmount}
                            onChange={(e) => {
                                setCustomAmount(e.target.value);
                                setSelectedStake(null);
                            }}
                        />

                        <button className="py-2 rounded-xl bg-green-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                            Play
                        </button>
                    </div>
                    <hr className="border-t border-gray-500" />
                    <button className="py-2 rounded-xl bg-blue-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                        Casual Game
                    </button>
                </div>
            )}
        </div>
    );
}

export default PlayButton;