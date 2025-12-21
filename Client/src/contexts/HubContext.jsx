import { createContext, useContext, useState } from "react";
import * as signalR from "@microsoft/signalr";

const HubContext = createContext();

export const HubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);

    const startConnection = async () => {
        await stopConnection();

        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7182/game', {
                accessTokenFactory: () => localStorage.getItem('accessToken')
            })
            .configureLogging(signalR.LogLevel.None)
            .withAutomaticReconnect()
            .build();

        await newConnection.start();
        setConnection(newConnection);

        return newConnection;
    };

    const stopConnection = async () => {
        if (connection) {
            await connection.stop();
            setConnection(null);
        }
    };

    return (
        <HubContext.Provider value={{ connection, startConnection, stopConnection }}>
            {children}
        </HubContext.Provider>
    );
};

export const useHub = () => useContext(HubContext);