import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import TopPlayers from '../components/GamePage/TopPlayers';
import GameInfo from '../components/GamePage/GameInfo';
import Matches from '../components/GamePage/Matches';
import GameRules from '../components/GamePage/GameRules';
import Loading from '../components/Loading';
import useApiRequest from '../hooks/useApiRequest';
import { useHub } from '../contexts/HubContext';

function GamePage() {
    const { game } = useParams();
    const apiRequest = useApiRequest();
    const { joinGame } = useHub();
    const [isLoading, setIsLoading] = useState(true);
    const [gameData, setGameData] = useState({});
    
    useEffect(() => {
        if (!gameData?.gameType) return;

        const initiateConnection = async () => {
            await joinGame(gameData.gameType);
        };

        initiateConnection();
    }, [gameData]);

    useEffect(() => {
        const receiveData = async () => {
            const gameData = await apiRequest('games', 'getGameData', 'POST', false, false, game);
            
            setIsLoading(false);
            setGameData(gameData);
        };

        receiveData();
    }, []);

    useEffect(() => {
        document.title = gameData.name;
    }, [gameData]);

    return (
        <section className="flex-1 p-6 space-y-6">
            {isLoading ? <Loading size={'small'} /> :
                <>
                    <div className="grid grid-cols-2 gap-6">
                        <GameInfo gameData={gameData} />
                        <TopPlayers />
                    </div>
                    <Matches gameType={gameData.gameType} />
                    <GameRules game={gameData.name} />
                </>}
        </section>
    );
}

export default GamePage;