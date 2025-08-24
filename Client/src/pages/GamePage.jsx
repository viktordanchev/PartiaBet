import React from 'react';
import ChessImg from '../assets/images/chess.jpg';
import BackgammonImg from '../assets/images/backgammon.png';
import BeloteImg from '../assets/images/belote.png';
import SixtySixImg from '../assets/images/sixty-six.png';
import TopPlayers from '../components/GamePage/TopPlayers';
import GameInfo from '../components/GamePage/GameInfo';
import Matches from '../components/GamePage/Matches';
import GameRules from '../components/GamePage/GameRules';

function GamePage({ game }) {
    const gameImages = {
        Chess: ChessImg,
        Backgammon: BackgammonImg,
        Belote: BeloteImg,
        'Sixty-Six': SixtySixImg,
    };
    
    return (
        <section className="flex-grow grid grid-cols-2 gap-6">
            <GameInfo
                gameName={game}
                gameImg={gameImages[game]}
            />
            <TopPlayers />
            <Matches />
            <GameRules game={game} />
        </section>
    );
}

export default GamePage;