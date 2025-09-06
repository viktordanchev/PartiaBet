﻿import React, { createContext, useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const navigate = useNavigate();
    const [token, setToken] = useState(() => localStorage.getItem('accessToken'));
    const [isAuthenticated, setIsAuthenticated] = useState(!!token);

    const updateToken = (token) => {
        localStorage.setItem('accessToken', token);
        setToken(token);
        setIsAuthenticated(true);
    };

    const removeToken = () => {
        localStorage.removeItem('accessToken');
        setIsAuthenticated(false);
        navigate('/');
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