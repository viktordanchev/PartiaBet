import { createContext, useContext, useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "./AuthContext";

const MatchHubContext = createContext();

export const MatchHubProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const [newPlayer, setNewPlayer] = useState(null);
    const [newMove, setNewMove] = useState(null);
    const [newMatch, setNewMatch] = useState(null);
    const { isAuthenticated } = useAuth();

    useEffect(() => {
        if (!connection) return;

        connection.on("ReceiveMatch", handleReceiveMatch);
        connection.on("ReceiveMove", handleReceiveMove);
        connection.on("ReceiveNewPlayer", handleReceiveNewPlayer);

        return () => {
            connection.off("ReceiveMatch", handleReceiveMatch);
            connection.off("ReceiveMove", handleReceiveMove);
            connection.off("ReceiveNewPlayer", handleReceiveNewPlayer);
        };
    }, [connection]);

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

    const handleReceiveMatch = (match) => {
        setNewMatch(match);
    };

    const handleReceiveMove = (moveData, newPlayerId, duration) => {
        setNewMove({ moveData, newPlayerId, duration });
    };

    const handleReceiveNewPlayer = (player) => {
        setNewPlayer(player);
    };

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
        <MatchHubContext.Provider value={{ connection, joinGame, newPlayer, newMove, newMatch }}>
            {children}
        </MatchHubContext.Provider>
    );
};

export const useMatchHub = () => useContext(MatchHubContext);