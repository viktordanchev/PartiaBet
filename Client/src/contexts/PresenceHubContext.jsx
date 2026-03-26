import { createContext, useContext, useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from './AuthContext';

const PresenceHubContext = createContext();

export const PresenceHubProvider = ({ children }) => {
    const { isAuthenticated } = useAuth();
    const [connection, setConnection] = useState(null);

    useEffect(() => {
        const createConnection = async () => {
            const newConnection = await startConnection();
            setConnection(newConnection);
        };

        createConnection();
    }, [isAuthenticated]);

    const stopConnection = async () => {
        if (connection) {
            await connection.stop();
            setConnection(null);
        }
    };

    const startConnection = async () => {
        await stopConnection();

        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7182/presence', {
                accessTokenFactory: () => localStorage.getItem('accessToken')
            })
            .configureLogging(signalR.LogLevel.None)
            .withAutomaticReconnect()
            .build();

        await newConnection.start();
        setConnection(newConnection);

        return newConnection;
    };

    return (
        <PresenceHubContext.Provider value={{ connection, startConnection, stopConnection }}>
            {children}
        </PresenceHubContext.Provider>
    );
};

export const usePresenceHub = () => useContext(PresenceHubContext);