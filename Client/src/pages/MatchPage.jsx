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

        const handleRemovePlayer = (playerId) => {
            setMatchData(prev => ({
                ...prev,
                players: prev.players.filter(player => player.id !== playerId)
            }));
        };

        const leaveMatch = async () => {
            await connection.invoke("LeaveMatch", matchId);
        };

        const handleBeforeUnload = () => {
            leaveMatch();
        };

        connection.on("ReceiveNewPlayer", handleReceiveNewPlayer);
        connection.on("RemovePlayer", handleRemovePlayer);
        window.addEventListener("beforeunload", handleBeforeUnload);

        return () => {
            connection.off("ReceiveNewPlayer", handleReceiveNewPlayer);
            connection.off("RemovePlayer", handleRemovePlayer);
            window.removeEventListener("beforeunload", handleBeforeUnload);

            leaveMatch();
        };
    }, [connection]);

    useEffect(() => {
        const receiveData = async () => {
            let [matchData, skins] = await Promise.all([
                apiRequest('matches', 'getMatchData', 'POST', true, false, matchId),
                apiRequest('matches', 'getSkins', 'GET', true, false)
            ]);

            setIsLoading(false);

            const updatedPieces = matchData.board.pieces.map(p => {
                const skin = skins.find(s => s.type === p.type);

                return {
                    ...p,
                    imageUrl: p.isWhite ? skin.white : skin.black
                };
            });

            matchData = {
                ...matchData,
                board: {
                    ...matchData.board,
                    pieces: updatedPieces
                }
            };

            setMatchData(matchData);
        };

        receiveData();
        localStorage.setItem('currentMatchId', matchId);
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
                        <Spectators peopleCount={0} />
                        <p className="text-2xl font-semibold text-gray-300">Bet: {matchData.betAmount}$</p>
                    </div>
                    {renderGame(matchData)}
                </>}
        </section>
    );
};

export default MatchPage;