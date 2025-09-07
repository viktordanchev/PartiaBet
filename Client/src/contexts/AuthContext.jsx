﻿import React, { createContext, useContext, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [token, setToken] = useState(() => localStorage.getItem('accessToken'));
    const [isAuthenticated, setIsAuthenticated] = useState(!!token);

    const updateToken = (token) => {
        localStorage.setItem('accessToken', token);
        setToken(token);
        setIsAuthenticated(true);
    };

    const removeToken = () => {
        localStorage.removeItem('accessToken');
        setToken(null);
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{
            token,
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