import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import useApiRequest from '../hooks/useApiRequest';

import Loading from '../components/Loading';
import PlayerInfo from '../components/playerPage/PlayerInfo';
import PlayerStats from '../components/playerPage/PlayerStats';
import PlayerGames from '../components/playerPage/PlayerGames';

function PlayerPage() {
    const { playerId } = useParams();
    const apiRequest = useApiRequest();
    const [isLoading, setIsLoading] = useState(true);
    const [playerData, setPlayerData] = useState(null);

    useEffect(() => {
        if (playerData) {
            document.title = playerData.username;
        }
    }, [playerData]);
    
    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);
            const playerData = await apiRequest('friends', 'getPlayer', 'POST', true, false, playerId);
            setIsLoading(false);
    
            setPlayerData(playerData);
        };
    
        receiveData();
    }, [playerId]);

    if (isLoading) {
        return (
            <Loading size={'big'} />
        );
    }

    return (
        <section className="flex-1 p-6 flex flex-col justify-start items-center gap-6">

            <div className="grid grid-cols-2 gap-6">
                <PlayerInfo playerData={playerData} />
                <PlayerStats playerData={playerData} />
            </div>

            <PlayerGames games={playerData.gamesStats} />

        </section>
    );
}

export default PlayerPage;