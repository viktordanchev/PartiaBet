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

    return (
        <HubContext.Provider value={{ connection }}>
            {children}
        </HubContext.Provider>
    );
};

export const useHub = () => useContext(HubContext);