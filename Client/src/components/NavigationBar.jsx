import React from 'react';
import { NavLink } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHouse, faMedal, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';

const NavigationBar = () => {
    return (
        <section className="w-80 bg-gray-900 text-xl font-medium text-gray-300">
            <nav className="fixed top-24 left-0 h-full w-80 flex flex-col space-y-3 p-3">
                <NavLink to="/"
                    className={({ isActive }) =>
                        `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                    }
                    end>
                    <FontAwesomeIcon icon={faHouse} /><p>Home</p>
                </NavLink>
                <NavLink to="/leaderboard"
                    className={({ isActive }) =>
                        `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                    }>
                    <FontAwesomeIcon icon={faMedal} /><p>Leaderboard</p>
                </NavLink>
                <NavLink to="/friends"
                    className={({ isActive }) =>
                        `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                    }>
                    <FontAwesomeIcon icon={faUserGroup} /><p>Friends</p>
                </NavLink>
                <NavLink to="/profile"
                    className={({ isActive }) =>
                        `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                    }>
                    <FontAwesomeIcon icon={faUser} /><p>Profile</p>
                </NavLink>
            </nav>
        </section>
    );
};

export default NavigationBar;