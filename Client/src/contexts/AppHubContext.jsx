import { createContext, useContext, useEffect, useState } from "react";
import { useAuth } from "./AuthContext";
import { createConnection } from '../services/signalR/signalRClient';

const AppHubContext = createContext();

export const AppHubProvider = ({ children }) => {
    const { isAuthenticated } = useAuth();
    const [connection, setConnection] = useState(null);

    useEffect(() => {
        const startConnection = async () => {
            const newConnection = createConnection();

            await newConnection.start();
            setConnection(newConnection);

            const matchId = sessionStorage.getItem("connection-matchId");
            const gameType = sessionStorage.getItem("connection-gameType");

            if (matchId) {
                await newConnection.invoke("JoinMatchGroup", matchId);
            }

            if (gameType) {
                await newConnection.invoke("JoinGameGroup", gameType);
            }
        };

        startConnection();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, [isAuthenticated]);

    const stopConnection = async () => {
        if (!connection) return;

        await connection.stop();
    };

    return (
        <AppHubContext.Provider value={{ connection, stopConnection }}>
            {children}
        </AppHubContext.Provider>
    );
};

export const useAppHub = () => useContext(AppHubContext);