import React from 'react';
import { NavLink } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHouse, faMedal, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';
import Logo from '../assets/images/logo.png';

const NavigationBar = () => {
    return (
        <section className="w-80 bg-gray-900">
            <div className="fixed top-0 left-0 h-full w-80 text-xl font-medium text-gray-300">
                <a href="/" className="text-center">
                    <img src={Logo} className="p-5" />
                </a>
                <nav className="flex flex-col space-y-3 p-3">
                    <NavLink
                        to="/"
                        className={({ isActive }) =>
                            `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                        }
                        end>
                        <FontAwesomeIcon icon={faHouse} /><p>Home</p>
                    </NavLink>
                    <NavLink
                        to="/leaderboard"
                        className={({ isActive }) =>
                            `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                        }>
                        <FontAwesomeIcon icon={faMedal} /><p>Leaderboard</p>
                    </NavLink>
                    <NavLink
                        to="/friends"
                        className={({ isActive }) =>
                            `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                        }>
                        <FontAwesomeIcon icon={faUserGroup} /><p>Friends</p>
                    </NavLink>
                    <NavLink
                        to="/profile"
                        className={({ isActive }) =>
                            `rounded-2xl p-3 flex items-center space-x-2 hover:bg-gray-900 hover:cursor-pointer ${isActive ? 'bg-gray-900 text-maincolor' : 'bg-gray-800'}`
                        }>
                        <FontAwesomeIcon icon={faUser} /><p>Profile</p>
                    </NavLink>
                </nav>
            </div>
        </section>
    );
};

export default NavigationBar;