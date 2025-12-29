﻿import React, { createContext, useContext, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('accessToken'));
    
    const updateToken = (newToken) => {
        localStorage.setItem('accessToken', newToken);

        if (newToken) {
            setIsAuthenticated(true);
        } else {
            setIsAuthenticated(false);
        }
    };

    const removeToken = () => {
        localStorage.removeItem('accessToken');
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{
            isAuthenticated,
            updateToken,
            removeToken
        }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    return useContext(AuthContext);
};