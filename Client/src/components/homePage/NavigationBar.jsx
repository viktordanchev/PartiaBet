import React from 'react';

const NavigationBar = () => {
    return (
        <nav className="w-96 rounded-xl bg-gray-900 border border-maincolor shadow shadow-maincolor">
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