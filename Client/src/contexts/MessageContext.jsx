import React, { createContext, useContext, useState } from 'react';

const MessageContext = createContext();

export function MessageProvider({ children }) {
    const [message, setMessage] = useState('');
    const [type, setType] = useState('');

    switch (type) {
        case 'message':
            color = 'text-green-500';
            break;
        case 'error':
            color = 'text-red-500';
            break;
    }

    const showMessage = (msg, msgType) => {
        setMessage(msg);
        setType(msgType);

        setTimeout(() => {
            setMessage('');
        }, 5000);
    };

    return (
        <MessageContext.Provider value={{ showMessage }}>
            {message &&
                <div className="fixed z-40 top-6 left-1/2 transform -translate-x-1/2 text-xl bg-gray-200 border-2 border-green-600 rounded-xl p-3 text-gray-800 ranimate-bounce-left-right">
                    {message}
                </div>}
            {children}
        </MessageContext.Provider>
    );
}

export const useMessage = () => useContext(MessageContext);