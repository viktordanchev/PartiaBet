import { createContext, useContext, useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "./AuthContext";

const HubContext = createContext();

export const HubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const { isAuthenticated } = useAuth();

    useEffect(() => {
        const startNewConnection = async () => {
            const newConnection = await startConnection();
            const gameId = sessionStorage.getItem("gameId");

            if (gameId) {
                await newConnection.invoke("JoinGame", gameId);
            }
        };

        startNewConnection();
    }, [isAuthenticated]);

    const joinGame = async (gameId) => {
        const newConnection = await startConnection();

        sessionStorage.setItem("gameId", gameId);
        await newConnection.invoke("JoinGame", gameId.toString());
    };

    const stopConnection = async () => {
        if (connection) {
            await connection.stop();
            setConnection(null);
        }
    };

    const startConnection = async () => {
        await stopConnection();

        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7182/match', {
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
        <HubContext.Provider value={{ connection, joinGame, stopConnection }}>
            {children}
        </HubContext.Provider>
    );
};

export const useHub = () => useContext(HubContext);