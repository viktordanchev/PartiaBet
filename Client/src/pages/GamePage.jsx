import React from 'react';
import { useParams } from "react-router-dom";
import TopPlayers from '../components/GamePage/TopPlayers';
import GameInfo from '../components/GamePage/GameInfo';
import Matches from '../components/GamePage/Matches';
import GameRules from '../components/GamePage/GameRules';
import { gamesData } from '../constants/gamesData';

function GamePage() {
    const { game } = useParams();
    const gameData = gamesData[game];
    
    return (
        <section className="flex-1 p-6 grid grid-cols-2 gap-6">
            <GameInfo gameData={gameData}/>
            <TopPlayers />
            <Matches />
            <GameRules game={game} />
        </section>
    );
}

export default GamePage;