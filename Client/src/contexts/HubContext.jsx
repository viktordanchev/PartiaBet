import { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

const HubContext = createContext();

export const HubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    var apiUrl = "https://localhost:7182";
    
    useEffect(() => {
        let newConnection;

        const startConnection = async () => {
            newConnection = new signalR.HubConnectionBuilder()
                .withUrl(`${apiUrl}/game`)
                .configureLogging(signalR.LogLevel.None)
                .withAutomaticReconnect()
                .build();

            await newConnection.start();
            setConnection(newConnection);
        };

        startConnection();

        return () => {
            if (newConnection) {
                newConnection.stop();
            }
        };
    }, []);

    const changeConnection = async (gameType) => {
        if (connection) {
            connection.off();
            await connection.stop();
        }
        
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${apiUrl}/${gameType}`)
            .configureLogging(signalR.LogLevel.None)
            .withAutomaticReconnect()
            .build();

        await newConnection.start();

        setConnection(newConnection);
    };

    return (
        <HubContext.Provider value={{ connection, changeConnection }}>
            {children}
        </HubContext.Provider>
    );
};

export const useHub = () => useContext(HubContext);