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
    const { connection, newPlayer, removedPlayer, matchStarted } = useMatchHub();
    const apiRequest = useApiRequest();
    const [isLoading, setIsLoading] = useState(true);
    const [match, setMatch] = useState(null);
    const [isStarted, setIsStarted] = useState(false);

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
            await new Promise(resolve => setTimeout(resolve, 3000));
            const data = await apiRequest('matches', 'getMatch', 'POST', true, false, matchId);
            if (!data) return;

            setIsLoading(false);
            setMatch(data);

            if (data.status === "Ongoing") {
                setIsStarted(true);
            }
        };

        receiveData();
    }, [matchStarted]);

    useEffect(() => {
        return () => {
            connection.invoke("LeaveMatch", matchId);
        };
    }, []);

    const renderGame = (matchData) => {
        switch (game) {
            case "chess":
                return <ChessMatch data={matchData} />;
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
                    {!isStarted ? <LobbyList match={match} /> :
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