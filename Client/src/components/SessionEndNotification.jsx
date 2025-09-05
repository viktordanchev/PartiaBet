import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClock } from '@fortawesome/free-regular-svg-icons';
import { useAuthContext } from '../contexts/AuthContext';

function SessionEndNotification() {
    const { isSessionEnd, setIsSessionEnd } = useAuthContext();

    const handleClose = () => {
        localStorage.removeItem('accessToken');
        setIsSessionEnd(false);
    };

    return (
        <>
            {isSessionEnd && (
                <div className="fixed left-6 bottom-6 flex flex-col items-center justify-between p-3 text-gray-700 shadow-2xl shadow-gray-400 animate-bounce-left-right z-50 rounded-xl h-36 w-72 bg-gray-200 bg-opacity-95 border border-zinc-500 sm:w-auto sm:left-3 sm:bottom-3 sm:right-3">
                    <div className="h-full w-full flex items-center justify-evenly space-x-3">
                        <FontAwesomeIcon icon={faClock} className="text-3xl" />
                        <div>
                            <p className="text-lg text-center">Session has ended</p>
                            <p className="text-base font-thin">Login to create new one.</p>
                        </div>
                    </div>
                    <div className="w-full flex flex-row justify-evenly">
                        <a href="/login"
                            className="bg-blue-500 border-2 border-blue-500 hover:bg-white hover:text-blue-500 text-white font-medium py-1 px-2 rounded"
                            onClick={handleClose}>
                            Login
                        </a>
                        <button className="bg-blue-500 border-2 border-blue-500 hover:bg-white hover:text-blue-500 text-white font-medium py-1 px-2 rounded"
                            onClick={handleClose}>
                            Close
                        </button>
                    </div>
                </div>
            )}
        </>
    );
}

export default SessionEndNotification;