import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";

import Loading from '../components/Loading';
import PlayerInfo from '../components/playerPage/PlayerInfo';
import PlayerStats from '../components/playerPage/PlayerStats';
import PlayerGames from '../components/playerPage/PlayerGames';

function PlayerPage() {
    const { playerId } = useParams();
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
            await new Promise(resolve => setTimeout(resolve, 1500));
            const playerData = {
                username: "Viktordanchev123",
                avatar: "https://i.pravatar.cc/150",
                isFriend: true,
                games: [
                    { name: "Chess", wins: 42, losses: 18, rating: 1720 },
                    { name: "Checkers", wins: 30, losses: 25, rating: 1605 },
                    { name: "TicTacToe", wins: 120, losses: 40, rating: 1950 }
                ]
            };
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

            <PlayerGames games={playerData.games} />

        </section>
    );
}

export default PlayerPage;