import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/MatchPage/Chess/ChessMatch';
import Spectators from '../components/MatchPage/Spectators';
import useApiRequest from '../hooks/useApiRequest';
import { useHub } from '../contexts/HubContext';

const MatchPage = () => {
    const { game, matchId } = useParams();
    const { connection } = useHub();
    const apiRequest = useApiRequest();
    const [isLoading, setIsLoading] = useState(true);
    const [matchData, setMatchData] = useState(null);

    useEffect(() => {
        if (!connection) return;

        const handleReceiveNewPlayer = (player) => {
            setMatchData(prev => ({
                ...prev,
                players: [player, ...prev.players]
            }));
        };

        connection.on("ReceiveNewPlayer", handleReceiveNewPlayer);

        return () => {
            connection.off("ReceiveNewPlayer", handleReceiveNewPlayer);
        };
    }, [connection]);

    useEffect(() => {
        const receiveData = async () => {
            const [matchData, skins] = await Promise.all([
                apiRequest('matches', 'getMatchData', 'POST', true, false, matchId),
                apiRequest('matches', 'getSkins', 'GET', true, false)
            ]);
            sessionStorage.setItem('currentMatchId', matchId);

            setIsLoading(false);

            const fullMatchData = {
                ...matchData,
                skins: skins
            };

            setMatchData(fullMatchData);
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
                    <div className="flex flex-col items-end gap-3">
                        <Spectators peopleCount={matchData.spectatorsCount} />
                        <p className="text-2xl font-semibold text-gray-300">Bet: {matchData.betAmount}$</p>
                    </div>
                    {renderGame(matchData)}
                </>}
        </section>
    );
};

export default MatchPage;