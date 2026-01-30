import { createContext, useContext, useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const MatchHubContext = createContext();

export const MatchHubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const [newPlayer, setNewPlayer] = useState(null);
    const [newMove, setNewMove] = useState(null);
    const [newMatch, setNewMatch] = useState(null);
    const [leaverData, setLeaverData] = useState(null);
    const [removedPlayer, setRemovedPlayer] = useState(null);
    const [rejoinedPlayer, setRejoinedPlayer] = useState('');
    const [matchStarted, setMatchStarted] = useState('');
    const [resumeMatch, setResumeMatch] = useState('');
    
    useEffect(() => {
        const matchId = sessionStorage.getItem('connection-matchId');
        const gameType = sessionStorage.getItem('connection-gameType');
        
        const createConnection = async () => {
            const newConnection = await startConnection();

            if (matchId) {
                await newConnection.invoke("JoinMatchGroup", matchId);
            }

            if (gameType) {
                await newConnection.invoke("JoinGameGroup", gameType);
            }

            setConnection(newConnection);
        };

        if (matchId || gameType) {
            createConnection();
        }
    }, []);

    useEffect(() => {
        if (!connection) return;
        
        connection.on("MatchResumed", handleMatchResumed);
        connection.on("StartMatch", handleStartMatch);
        connection.on("RejoinPlayer", handleRejoinPlayer);
        connection.on("RemovePlayer", handleRemovePlayer);
        connection.on("RejoinCountdown", handleRejoinCountdown);
        connection.on("ReceiveMatch", handleReceiveMatch);
        connection.on("ReceiveMove", handleReceiveMove);
        connection.on("ReceivePlayer", handleReceivePlayer);
    }, [connection]);

    const handleMatchResumed = (matchId) => {
        setResumeMatch(matchId);
    };

    const handleStartMatch = (matchId) => {
        setMatchStarted(matchId);
    };

    const handleRejoinPlayer = (playerId) => {
        setRejoinedPlayer(playerId);
    };

    const handleRemovePlayer = (matchId, playerId) => {
        setRemovedPlayer({ matchId, playerId });
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

    const handleReceivePlayer = (matchId, player) => {
        setNewPlayer({ matchId, player });
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

    const ensureConnection = async () => {
        if (connection) return connection;
        return await startConnection();
    };

    return (
        <MatchHubContext.Provider value={{ connection, ensureConnection, stopConnection, newPlayer, newMove, newMatch, leaverData, removedPlayer, rejoinedPlayer, matchStarted, resumeMatch }}>
            {children}
        </MatchHubContext.Provider>
    );
};

export const useMatchHub = () => useContext(MatchHubContext);