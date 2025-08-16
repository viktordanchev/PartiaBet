import React from 'react';
import GameCard from './GameCard';
import ChessImg from '../../assets/images/chess.jpg';

const GamesList = () => {
    return (
        <ul className="w-full flex justify-center items-center gap-4">
            <GameCard
                gameImg={ChessImg}
                gameName={'Chess'}
                playersCount={12}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Backgammon'}
                playersCount={57}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Belote'}
                playersCount={144}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Sixty-Six'}
                playersCount={6}
            />
        </ul>
    );
};

export default GamesList;