import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/matchPage/chess/ChessMatch';
import Spectators from '../components/matchPage/Spectators';
import LobbyList from '../components/matchPage/waitingLobby/LobbyList';
import EndingScreen from '../components/matchPage/EndingScreen';
import useApiRequest from '../hooks/useApiRequest';
import { useAppHub } from '../contexts/AppHubContext';
import { useSignalREvent } from "../hooks/signalR/useSignalREvent";
import PausedMatch from '../components/MatchPage/PausedMatch';

const MatchPage = () => {
    const { game, matchId } = useParams();
    const { connection, stopConnection } = useAppHub();
    const apiRequest = useApiRequest();

    const [isLoading, setIsLoading] = useState(true);
    const [match, setMatch] = useState(null);
    const [isPaused, setIsPaused] = useState(false);
    const [isEnded, setIsEnded] = useState(false);

    //Load match
    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);

            const data = await apiRequest('matches', 'getMatch', 'POST', true, false, matchId);
           
            if (!data) return;

            setMatch(data);
            setIsPaused(data.status === 'Paused');
            setIsLoading(false);
        };

        receiveData();
    }, [matchStarted, resumeMatch, matchId]);

    useSignalREvent("EndMatch", () => {
        setIsEnded(true);
    });

    useSignalREvent("RejoinCountdown", () => {
        setIsPaused(true);
    });

    useSignalREvent("ReceivePlayer", (matchId, player) => {
        setMatch(prev => {
            if (!prev || prev.id !== matchId) return prev;

            const exists = prev.players.some(p => p.id === player.id);
            if (exists) return prev;

            return {
                ...prev,
                players: [...prev.players, player]
            };
        });
    });

    useSignalREvent("RemovePlayer", (matchId, playerId) => {
        setMatch(prev => {
            if (!prev || prev.id !== matchId) return prev;

            const exists = prev.players.some(p => p.id === playerId);
            if (!exists) return prev;

            return {
                ...prev,
                players: prev.players.filter(p => p.id !== playerId)
            };
        });
    });

    useEffect(() => {
        return () => {
            if (connection) {
                stopConnection();
            }
        };
    }, [connection]);

    const renderGame = (matchData) => {
        switch (game) {
            case "chess":
                return <ChessMatch data={matchData} isPaused={isPaused} />;
            case "dota":
                return <div>Dota (TODO)</div>;
            case "csgo":
                return <div>CSGO (TODO)</div>;
            default:
                return <p>Game not supported</p>;
        }
    };

    if (isLoading) {
        return (
            <section className="flex-1 p-6 flex justify-center">
                <Loading size="small" />
            </section>
        );
    }

    return (
        <section className="flex-1 p-6 flex justify-center gap-3">
            <>
                {isEnded && matchEnd && (
                    <EndingScreen players={matchEnd.winners} />
                )}

                {match?.status === "Created" && (
                    <LobbyList match={match} />
                )}

                {isPaused && <PausedMatch />}

                {match?.status !== "Created" && (
                    <>
                        <div className="flex flex-col items-end gap-3">
                            <Spectators peopleCount={0} />
                            <p className="text-2xl font-semibold text-gray-300">
                                Bet: {match.betAmount}$
                            </p>
                        </div>

                        {renderGame(match)}
                    </>
                )}
            </>
        </section>
    );
};

export default MatchPage;