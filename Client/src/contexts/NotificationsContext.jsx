import React, { createContext, useContext, useState } from 'react';

const NotificationsContext = createContext();

export function NotificationsProvider({ children }) {
    const [message, setMessage] = useState('');
    const [type, setType] = useState('');

    const showMessage = (msg, msgType) => {
        setMessage(msg);
        setType(msgType);

        setTimeout(() => {
            setMessage('');
        }, 5000);
    };

    return (
        <NotificationsContext.Provider value={{ showMessage }}>
            {message &&
                <div className={`fixed z-40 top-6 left-1/2 transform -translate-x-1/2 text-xl bg-gray-200 border-2 rounded p-3 text-gray-800 animate-bounce-left-right ${type === 'message' ? 'border-green-600' : 'border-red-600'}`}>
                    {message}
                </div>}
            {children}
        </NotificationsContext.Provider>
    );
}

export const useNotifications = () => useContext(NotificationsContext);