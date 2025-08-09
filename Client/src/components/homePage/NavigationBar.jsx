import React from 'react';

const NavigationBar = () => {
    return (
        <nav className="w-1/3 bg-gray-100 rounded-xl opacity-45 border-2 border-gray-500">
            <ul>
                <li><a href="/">Home</a></li>
                <li><a href="/bets">Bets</a></li>
                <li><a href="/results">Results</a></li>
                <li><a href="/profile">Profile</a></li>
            </ul>
        </nav>
    );
};

export default NavigationBar;