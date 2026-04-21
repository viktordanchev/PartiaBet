import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/matchPage/chess/ChessMatch';
import Spectators from '../components/matchPage/Spectators';
import LobbyList from '../components/matchPage/waitingLobby/LobbyList';
import EndingScreen from '../components/matchPage/EndingScreen';
import useApiRequest from '../hooks/useApiRequest';
import { useAppHub } from '../contexts/AppHubContext';
import PausedMatch from '../components/MatchPage/PausedMatch';

const MatchPage = () => {
    const { game, matchId } = useParams();
    
    const {
        connection,
        stopConnection,
        matchState
    } = useAppHub();

    const {
        newPlayer,
        removedPlayer,
        matchStarted,
        leaverData,
        resumeMatch,
        matchEnd
    } = matchState;

    const apiRequest = useApiRequest();

    const [isLoading, setIsLoading] = useState(true);
    const [match, setMatch] = useState(null);
    const [isPaused, setIsPaused] = useState(false);
    const [isEnded, setIsEnded] = useState(false);

    //Load match
    useEffect(() => {
        console.log("Fetching match data...");
        const receiveData = async () => {
            setIsLoading(true);

            const data = await apiRequest(
                'matches',
                'getMatch',
                'POST',
                true,
                false,
                matchId
            );
           
            if (!data) return;

            setMatch(data);
            setIsPaused(data.status === 'Paused');
            setIsLoading(false);
        };

        receiveData();
    }, [matchStarted, resumeMatch, matchId]);

    //Match end
    useEffect(() => {
        if (!matchEnd) return;

        setIsEnded(true);
    }, [matchEnd]);

    //Pause match
    useEffect(() => {
        if (!leaverData) return;

        setIsPaused(true);
    }, [leaverData]);

    //Add player
    useEffect(() => {
        if (!match || !newPlayer) return;

        setMatch(prev => {
            if (!prev) return prev;

            const exists = prev.players.some(p => p.id === newPlayer.player.id);
            if (exists) return prev;

            return {
                ...prev,
                players: [...prev.players, newPlayer.player]
            };
        });
    }, [newPlayer]);

    //Remove player
    useEffect(() => {
        if (!match || !removedPlayer) return;

        setMatch(prev => {
            if (!prev) return prev;

            return {
                ...prev,
                players: prev.players.filter(p => p.id !== removedPlayer.playerId)
            };
        });
    }, [removedPlayer]);

    //Cleanup on unmount
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