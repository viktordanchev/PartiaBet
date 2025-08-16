import React from 'react';

const NavigationBar = () => {
    return (
        <nav className="w-90 bg-gray-900 p-3 text-xl font-medium">
            <ul className="text-gray-300 space-y-3">
                <li className="bg-gray-800 rounded-2xl p-3 hover:bg-gray-900"><a href="/">Home</a></li>
                <li className="bg-gray-800 rounded-2xl p-3 hover:bg-gray-900"><a href="/bets">Bets</a></li>
                <li className="bg-gray-800 rounded-2xl p-3 hover:bg-gray-900"><a href="/results">Results</a></li>
                <li className="bg-gray-800 rounded-2xl p-3 hover:bg-gray-900"><a href="/profile">Profile</a></li>
            </ul>
        </nav>
    );
};

export default NavigationBar;