import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/MatchPage/Chess/ChessMatch';
import Spectators from '../components/MatchPage/Spectators';
import LobbyList from '../components/MatchPage/WaitingLobby/LobbyList';
import useApiRequest from '../hooks/useApiRequest';
import { useMatchHub } from '../contexts/MatchHubContext';

const MatchPage = () => {
    const { game, matchId } = useParams();
    const { connection, stopConnection, newPlayer, removedPlayer, matchStarted, leaverData, resumeMatch } = useMatchHub();
    const apiRequest = useApiRequest();
    const [isLoading, setIsLoading] = useState(true);
    const [match, setMatch] = useState(null);
    const [isPaused, setIsPaused] = useState(false);

    useEffect(() => {
        if (!match) return;

        setIsPaused(true);
    }, [leaverData]);

    useEffect(() => {
        if (!match) return;
        
        setMatch(prev => ({
            ...prev,
            players: [...prev.players, newPlayer.player]
        }));
    }, [newPlayer]);

    useEffect(() => {
        if (!match) return;

        setMatch(prev => ({
            ...prev,
            players: prev.players.filter(p => p.id !== removedPlayer.playerId)
        }));
    }, [removedPlayer]);

    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);

            const data = await apiRequest('matches', 'getMatch', 'POST', true, false, matchId);

            setIsLoading(false);
            setMatch(data);
            setIsPaused(data.status === 'Paused');
        };

        receiveData();
    }, [matchStarted, resumeMatch]);

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
                return <DotaMatch data={matchData} />;
            case "csgo":
                return <CsgoMatch data={matchData} />;
            default:
                return <p>Game not supported</p>;
        }
    };

    return (
        <section className="flex-1 p-6 flex justify-center gap-3">
            {isLoading ? <Loading size={'small'} /> :
                <>
                    {match?.status === "Created" && <LobbyList match={match} />}

                    {isPaused && (
                        <div className="fixed inset-0 bg-black/50 z-50 flex items-center justify-center">
                            <div className="bg-gray-900 p-6 rounded-xl text-center">
                                <p className="text-xl font-semibold text-white">
                                    Match is paused
                                </p>
                                <p className="text-gray-400 mt-2">
                                    Waiting for players to rejoin...
                                </p>
                            </div>
                        </div>
                    )}

                    {match?.status !== "Created" &&
                        <>
                            <div className="flex flex-col items-end gap-3">
                                <Spectators peopleCount={0} />
                                <p className="text-2xl font-semibold text-gray-300">Bet: {match.betAmount}$</p>
                            </div>
                            {renderGame(match)}
                        </>}
                </>}
        </section>
    );
};

export default MatchPage;