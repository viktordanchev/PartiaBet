import { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

const HubContext = createContext();

export const HubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const [playersCount, setPlayersCount] = useState({});
    
    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7182/game")
            .configureLogging(signalR.LogLevel.None)
            .withAutomaticReconnect()
            .build();
        
        newConnection.start()
            .then(() => {
                newConnection.on("UpdatePlayerCount", (gameName, playersCount) => {
                    setPlayersCount(prev => ({ ...prev, [gameName]: playersCount }));
                });
            })
            .catch();

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, []);

    const joinMatch = (matchData) => {
        connection.invoke("JoinMatch", matchData);
    };

    return (
        <HubContext.Provider value={{ connection, playersCount, joinMatch }}>
            {children}
        </HubContext.Provider>
    );
};

export const useHub = () => useContext(HubContext);