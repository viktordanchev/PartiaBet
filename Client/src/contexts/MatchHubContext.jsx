import { createContext, useContext, useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const MatchHubContext = createContext();

export const MatchHubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const [newPlayer, setNewPlayer] = useState(null);
    const [newMove, setNewMove] = useState(null);
    const [newMatch, setNewMatch] = useState(null);
    const [leaverData, setLeaverData] = useState(null);
    const [removedPlayer, setRemovedPlayer] = useState('');
    const [rejoinedPlayer, setRejoinedPlayer] = useState('');

    useEffect(() => {
        if (!connection) return;
        
        connection.on("RejoinPlayer", handleRejoinPlayer);
        connection.on("RemovePlayer", handleRemovePlayer);
        connection.on("RejoinCountdown", handleRejoinCountdown);
        connection.on("ReceiveMatch", handleReceiveMatch);
        connection.on("ReceiveMove", handleReceiveMove);
        connection.on("ReceiveNewPlayer", handleReceiveNewPlayer);
    }, [connection]);
    
    const handleRejoinPlayer = (playerId) => {
        setRejoinedPlayer(playerId);
    };

    const handleRemovePlayer = (playerId) => {
        setRemovedPlayer(playerId);
    };

    const handleRejoinCountdown = (playerId, timeLeft) => {
        setLeaverData({ playerId, timeLeft });
    };

    const handleReceiveMatch = (match) => {
        setNewMatch(match);
    };

    const handleReceiveMove = (moveData, newPlayerId, duration) => {
        setNewMove({ moveData, newPlayerId, duration });
    };

    const handleReceiveNewPlayer = (player, matchId) => {
        setNewPlayer({ player, matchId });
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
        <MatchHubContext.Provider value={{ connection, startConnection, newPlayer, newMove, newMatch, leaverData, removedPlayer, rejoinedPlayer }}>
            {children}
        </MatchHubContext.Provider>
    );
};

export const useMatchHub = () => useContext(MatchHubContext);