import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClock } from '@fortawesome/free-regular-svg-icons';
import { useAuth } from '../contexts//AuthContext';

function SessionEndNotification() {
    const { removeAccessToken } = useAuth();

    return (
        <>
            {localStorage.getItem('accessToken') === undefined && (
                <div className="fixed z-40 left-6 bottom-6 flex flex-col items-center justify-between p-3 text-gray-800 animate-bounce-left-right rounded h-36 w-72 bg-gray-200 bg-opacity-95 border-2 border-maincolor">
                    <div className="h-full w-full flex items-center justify-evenly space-x-3">
                        <FontAwesomeIcon icon={faClock} className="text-3xl" />
                        <div>
                            <p className="text-lg text-center">Session has ended</p>
                            <p className="text-base font-thin">Login to create new one.</p>
                        </div>
                    </div>
                    <div className="w-full flex flex-row justify-evenly">
                        <a href="/login"
                            className="bg-maincolor text-gray-900 font-medium py-2 px-4 rounded hover:bg-[#81e4dc]"
                            onClick={() => removeAccessToken()}>
                            Login
                        </a>
                        <button className="bg-maincolor text-gray-900 font-medium py-2 px-4 rounded hover:bg-[#81e4dc] hover:cursor-pointer"
                            onClick={() => removeAccessToken()}>
                            Close
                        </button>
                    </div>
                </div>
            )}
        </>
    );
}

export default SessionEndNotification;