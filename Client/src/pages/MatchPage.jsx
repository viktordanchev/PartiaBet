import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import useApiRequest from '../hooks/useApiRequest';
import { useAppHub } from '../contexts/AppHubContext';
import { useSignalREvent } from "../hooks/signalR/useSignalREvent";

import Loading from '../components/Loading';
import ChessMatch from '../components/matchPage/chess/ChessGame';
import Spectators from '../components/matchPage/Spectators';
import LobbyList from '../components/matchPage/waitingLobby/LobbyList';
import EndingScreen from '../components/matchPage/EndingScreen';
import PausedMatch from '../components/matchPage/PausedMatch';
import GameRenderer from '../components/matchPage/GameRenderer';

const MatchPage = () => {
    const { game, matchId } = useParams();
    const { connection, stopConnection } = useAppHub();
    const apiRequest = useApiRequest();

    const [isLoading, setIsLoading] = useState(true);
    const [match, setMatch] = useState(null);
    const [playersStatsInTheEnd, setFinalPlayerStats] = useState(null);
    const [pauseState, setPauseState] = useState({
        active: false,
        rejoinDeadline: null
    });

    const fetchMatch = async () => {
        setIsLoading(true);

        const data = await apiRequest('matches', 'getMatch', 'POST', true, false, matchId);
        if (!data) return;

        setMatch(data);
        setPauseState({ active: data.status === 'Paused', rejoinDeadline: data.rejoinDeadline });
        setIsLoading(false);
    };

    useEffect(() => {
        fetchMatch();
    }, [matchId]);

    useSignalREvent("EndMatch", (players) => {
        setFinalPlayerStats(players);
    });

    useSignalREvent("MatchResumed", () => {
        localStorage.removeItem('matchEndCountdown');
        fetchMatch();
    });

    useSignalREvent("StartMatch", () => {
        fetchMatch();
    });

    useSignalREvent("RejoinCountdown", (rejoinDeadline) => {
        setPauseState({ active: true, rejoinDeadline });
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
                {playersStatsInTheEnd &&
                    <EndingScreen players={playersStatsInTheEnd} />
                }

                {match?.status === "Created" &&
                    <LobbyList match={match} />
                }

                {pauseState.active && <PausedMatch rejoinDeadline={pauseState.rejoinDeadline} />}

                {match?.status !== "Created" && (
                    <>
                        <div className="flex flex-col items-end gap-3">
                            <Spectators peopleCount={0} />
                            <p className="text-2xl font-semibold text-gray-300">
                                Bet: {match.betAmount}$
                            </p>
                        </div>

                        <GameRenderer
                            game={game}
                            matchData={match}
                            isPaused={pauseState.active}
                        />
                    </>
                )}
            </>
        </section>
    );
};

export default MatchPage;