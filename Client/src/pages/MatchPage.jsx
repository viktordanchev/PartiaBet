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
    const { newPlayer, removedPlayer } = useMatchHub();
    const apiRequest = useApiRequest();
    const [isLoading, setIsLoading] = useState(true);
    const [matchData, setMatchData] = useState(null);
    const [isStarted, setIsStarted] = useState(false);

    useEffect(() => {
        if (!matchData) return;

        if (newPlayer) {
            setMatchData(prev => ({
                ...prev,
                players: [...prev.players, newPlayer]
            }));
        } else if (removedPlayer) {
            setMatchData(prev => ({
                ...prev,
                players: prev.players.filter(p => p.id !== removedPlayer.id)
            }));
        }
    }, [newPlayer, removedPlayer]);

    useEffect(() => {
        const receiveData = async () => {
            const matchData = await apiRequest('matches', 'getMatch', 'POST', true, false, matchId);
            if (!matchData) return;

            if (matchData.maxPlayersCount === matchData.players.length) {
                setIsStarted(true);
            }

            setIsLoading(false);
            setMatchData(matchData);
        };

        receiveData();
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
                    {!isStarted ? <LobbyList matchData={matchData} /> :
                        <>
                            <div className="flex flex-col items-end gap-3">
                                <Spectators peopleCount={0} />
                                <p className="text-2xl font-semibold text-gray-300">Bet: {matchData.betAmount}$</p>
                            </div>
                            {renderGame(matchData)}
                        </>}
                </>}
        </section>
    );
};

export default MatchPage;