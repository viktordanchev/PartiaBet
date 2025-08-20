import React from 'react';
import Logo from '../assets/images/logo.png';

const Header = () => {
    return (
        <header className="bg-gray-900 h-24 p-6 flex justify-end items-center">
            <div className="space-x-4">
                <a href="/login" className="bg-maincolor text-gray-800 font-medium py-3 px-6 rounded hover:bg-[#81e4dc]">
                    Login
                </a>
                <a href="/register" className="bg-maincolor text-gray-800 font-medium py-3 px-6 rounded hover:bg-[#81e4dc]">
                    Register
                </a>
            </div>
        </header>
    );
};

export default Header;