﻿import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';
import useRefreshToken from '../hooks/useRefreshToken';
import { useLoading } from './LoadingContext';
import Loading from '../components/Loading';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const navigate = useNavigate();
    const [token, setToken] = useState(() => localStorage.getItem("accessToken"));
    const [isAuthenticated, setIsAuthenticated] = useState(!!token);
    
    useEffect(() => {
        const tryRefreshToken = async () => {
            setIsAuthLoading(true);

            const isRefreshed = await refreshAccessToken();

            setIsAuthLoading(false);

            if (isRefreshed) {
                setIsAuthenticated(true);
            } else {
                setIsSessionEnd(isTokenExist);
                setIsAuthenticated(false);
            }
        };
        
        if (!isAuth && isTokenExist) {
            tryRefreshToken();
        }
    }, []);

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

    const update = (token) => {
        localStorage.setItem('accessToken', token);
    };

    const isStillAuth = async () => {
        let IsStill = false;
        const isAuth = useAuth();

        if (!isAuth) {
            setIsLoading(true);

            const isRefreshed = await refreshAccessToken();

            setIsLoading(false);

            if (isRefreshed) {
                IsStill = true;
            }
        } else {
            IsStill = true;
        }

        setIsSessionEnd(!IsStill);
        setIsAuthenticated(IsStill);

        if (!IsStill) navigate('/home');

        return IsStill;
    };

    return (
        <AuthContext.Provider value={{
            isAuthenticated,
            isSessionEnd,
            setIsSessionEnd,
            login,
            logout,
            update,
            isStillAuth
        }}>
            {isAuthLoading ? <div className="fixed h-full w-full"><Loading type={'big'} /></div> : children}
        </AuthContext.Provider>
    );
};

export const useAuthContext = () => {
    return useContext(AuthContext);
};