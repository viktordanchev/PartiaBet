import React from 'react';
import Logo from '../assets/images/logo.png';
import { useAuth } from '../contexts/AuthContext';
import Profile from './Profile';

const Header = () => {
    const { isAuthenticated, token } = useAuth();
   
    return (
        <header className="bg-gray-900 h-24 p-6 flex justify-between items-center">
            <a href="/" className="text-center">
                <img src={Logo} className="w-80" />
            </a>
            {isAuthenticated ? <Profile token={token} /> :
                <div className="space-x-4">
                    <a href="/login" className="bg-maincolor text-gray-900 font-medium py-3 px-6 rounded hover:bg-[#81e4dc]">
                        Login
                    </a>
                    <a href="/register" className="bg-maincolor text-gray-900 font-medium py-3 px-6 rounded hover:bg-[#81e4dc]">
                        Register
                    </a>
                </div>}
        </header>
    );
};

export default Header;