﻿import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const navigate = useNavigate();
    const [token, setToken] = useState(() => localStorage.getItem("accessToken"));
    const [isAuthenticated, setIsAuthenticated] = useState(!!token);
    
    

    const login = (token) => {
        localStorage.setItem('accessToken', token);
        setToken(token);
        setIsAuthenticated(true);
    };

    const logout = () => {
        localStorage.removeItem('accessToken');

        setIsAuthenticated(false);

        navigate('/');
    };

    return (
        <AuthContext.Provider value={{
            isAuthenticated,
            login,
            logout
        }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuthContext = () => {
    return useContext(AuthContext);
};