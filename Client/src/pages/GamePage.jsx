import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import TopPlayers from '../components/GamePage/TopPlayers';
import GameInfo from '../components/GamePage/GameInfo';
import Matches from '../components/GamePage/Matches';
import GameRules from '../components/GamePage/GameRules';

function GamePage() {
    const location = useLocation();
    const { gameData } = location.state;

    useEffect(() => {
        document.title = gameData.name;
    });

    return (
        <section className="flex-1 p-6 grid grid-cols-2 gap-6">
            <GameInfo gameData={gameData}/>
            <TopPlayers />
            <Matches gameType={gameData.gameType} />
            <GameRules game={gameData.name} />
        </section>
    );
}

export default GamePage;