import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/MatchPage/Chess/ChessMatch';
import Spectators from '../components/MatchPage/Spectators';
import useApiRequest from '../hooks/useApiRequest';

const MatchPage = () => {
    const { game, matchId } = useParams();
    const apiRequest = useApiRequest();
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const receiveData = async () => {
            const values = {
                game: game,
                matchId: matchId 
            };

            const matchData = await apiRequest('matches', 'getMatchData', 'POST', true, false, values);
            console.log(matchData);
        };

        receiveData();
    }, []);

    const renderGame = () => {
        switch (game) {
            case "chess":
                return <ChessMatch />;
            case "dota":
                return <DotaMatch />;
            case "csgo":
                return <CsgoMatch />;
            default:
                return <p>Game not supported</p>;
        }
    };

    return (
        <section className="flex-1 p-6 flex justify-center gap-3">
            {isLoading ? <Loading size={'small'} /> :
                <>
                    <div className="flex flex-col items-end gap-3">
                        <Spectators peopleCount={125} />
                        <p className="text-2xl font-semibold text-gray-300">Total 40$</p>
                    </div>
                    {renderGame()}
                </>}
        </section>
    );
};

export default MatchPage;