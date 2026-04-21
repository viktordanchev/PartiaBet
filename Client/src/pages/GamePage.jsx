import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import TopPlayers from '../components/gamePage/TopPlayers';
import GameInfo from '../components/gamePage/GameInfo';
import Matches from '../components/gamePage/Matches';
import GameRules from '../components/gamePage/GameRules';
import Loading from '../components/Loading';
import useApiRequest from '../hooks/useApiRequest';
import { useAppHub } from '../contexts/AppHubContext';

function GamePage() {
    const { game } = useParams();
    const apiRequest = useApiRequest();
    const { connection } = useAppHub();
    const [isLoading, setIsLoading] = useState(true);
    const [gameData, setGameData] = useState(null);

    useEffect(() => {
        const receiveData = async () => {
            const gameData = await apiRequest('games', 'getGameData', 'POST', false, false, game);

            await connection.invoke("JoinGameGroup", gameData.gameType);

            sessionStorage.setItem('connection-gameType', gameData.gameType);

            setIsLoading(false);
            setGameData(gameData);
        };
        
        if (connection)
            receiveData();
    }, [connection]);

    useEffect(() => {
        if (!gameData) return;

        document.title = gameData.name;
    }, [gameData]);

    if (isLoading) {
        return (
            <Loading size={'big'} />
        );
    }

    return (
        <section className="flex-1 p-6 space-y-6">
            <div className="grid grid-cols-2 gap-6">
                <GameInfo gameData={gameData} />
                <TopPlayers />
            </div>
            <Matches gameType={gameData.gameType} />
            <GameRules game={gameData.name} />
        </section>
    );
}

export default GamePage;