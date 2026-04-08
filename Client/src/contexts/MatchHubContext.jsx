import { createContext, useContext, useState, useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "./AuthContext";

const MatchHubContext = createContext();

export const MatchHubProvider = ({ children }) => {
    const { isAuthenticated } = useAuth();

    const [connection, setConnection] = useState(null);

    const [matchState, setMatchState] = useState({
        newPlayer: null,
        newMove: null,
        newMatch: null,
        leaverData: null,
        removedPlayer: null,
        matchStarted: "",
        resumeMatch: false,
        matchEnd: null
    });

    const handlersRef = useRef(matchState);

    useEffect(() => {
        handlersRef.current = matchState;
    }, [matchState]);

    useEffect(() => {
        const createConnection = async () => {
            const newConnection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7182/match", {
                    accessTokenFactory: () => localStorage.getItem("accessToken")
                })
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.None)
                .build();

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

        createConnection();
    }, [isAuthenticated]);

    useEffect(() => {
        if (!connection) return;

        const handleMatchEnd = (winners) => {
            updateState({ matchEnd: { winners } });
        };

        const handleMatchResumed = () => {
            updateState({ resumeMatch: !handlersRef.current.resumeMatch });
        };

        const handleStartMatch = (matchId) => {
            updateState({ matchStarted: matchId });
        };

        const handleRemovePlayer = (matchId, playerId) => {
            updateState({ removedPlayer: { matchId, playerId } });
        };

        const handleRejoinCountdown = (playerId, timeLeft) => {
            updateState({ leaverData: { playerId, timeLeft } });
        };

        const handleReceiveMatch = (match) => {
            updateState({ newMatch: match });
        };

        const handleReceiveMove = (gameBoard, newPlayerId, duration) => {
            updateState({
                newMove: { gameBoard, newPlayerId, duration }
            });
        };

        const handleReceivePlayer = (matchId, player) => {
            updateState({
                newPlayer: { matchId, player }
            });
        };

        connection.on("EndMatch", handleMatchEnd);
        connection.on("MatchResumed", handleMatchResumed);
        connection.on("StartMatch", handleStartMatch);
        connection.on("RemovePlayer", handleRemovePlayer);
        connection.on("RejoinCountdown", handleRejoinCountdown);
        connection.on("ReceiveMatch", handleReceiveMatch);
        connection.on("ReceiveMove", handleReceiveMove);
        connection.on("ReceivePlayer", handleReceivePlayer);

        return () => {
            connection.off("EndMatch", handleMatchEnd);
            connection.off("MatchResumed", handleMatchResumed);
            connection.off("StartMatch", handleStartMatch);
            connection.off("RemovePlayer", handleRemovePlayer);
            connection.off("RejoinCountdown", handleRejoinCountdown);
            connection.off("ReceiveMatch", handleReceiveMatch);
            connection.off("ReceiveMove", handleReceiveMove);
            connection.off("ReceivePlayer", handleReceivePlayer);
        };

    }, [connection]);

    const updateState = (updates) => {
        setMatchState(prev => ({ ...prev, ...updates }));
    };

    const stopConnection = async () => {
        if (connection) {
            await connection.stop();
            setConnection(null);
        }
    };

    return (
        <MatchHubContext.Provider value={{
            connection,
            stopConnection,
            ...matchState
        }}>
            {children}
        </MatchHubContext.Provider>
    );
};

export const useMatchHub = () => useContext(MatchHubContext);