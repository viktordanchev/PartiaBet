import React from 'react';
import { NavLink } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHouse, faMedal, faUser } from '@fortawesome/free-solid-svg-icons';

const NavigationBar = () => {
    return (
        <nav className="min-w-80 bg-gray-900 p-3 text-xl font-medium text-gray-300 flex flex-col space-y-3">
            <NavLink
                to="/"
                className={({ isActive }) =>
                    `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900' : 'bg-gray-800'}`
                }
                end
            >
                <FontAwesomeIcon icon={faHouse} /><p>Home</p>
            </NavLink>
            <NavLink
                to="/results"
                className={({ isActive }) =>
                    `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900' : 'bg-gray-800'}`
                }
            >
                <FontAwesomeIcon icon={faMedal} /><p>Leaderboard</p>
            </NavLink>
            <NavLink
                to="/bets"
                className={({ isActive }) =>
                    `rounded-2xl p-3 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900' : 'bg-gray-800'}`
                }
            >
                Bets
            </NavLink>
            <NavLink
                to="/profile"
                className={({ isActive }) =>
                    `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900' : 'bg-gray-800'}`
                }
            >
                <FontAwesomeIcon icon={faUser} /><p>Profile</p>
            </NavLink>
        </nav>
    );
};

export default NavigationBar;