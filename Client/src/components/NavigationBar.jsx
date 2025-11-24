import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHouse, faMedal, faUser, faUserGroup, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { useAuth } from '../contexts/AuthContext';
import Logo from '../assets/images/logo.png';

const NavigationBar = () => {
    const navigate = useNavigate();
    const { isAuthenticated, removeToken } = useAuth();

    const logout = () => {
        removeToken();
        navigate('/');
    };

    return (
        <section className="fixed h-full w-80 bg-gray-900 text-xl font-medium text-gray-300">
            <a href="/" className="block p-6">
                <img src={Logo} />
            </a>
            <nav className="flex flex-col space-y-3 p-3">
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
                {isAuthenticated &&
                    <>
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
                        <div className="rounded-2xl p-3 flex items-center space-x-2 text-red-400 bg-gray-800 hover:bg-gray-900 hover:cursor-pointer"
                            onClick={logout}>
                            <FontAwesomeIcon icon={faRightFromBracket} /><p>Logout</p>
                        </div>
                    </>}
            </nav>
        </section>
    );
};

export default NavigationBar;