import React from 'react';
import ChessGame from '../components/matchPage/chess/ChessGame';

const GameRenderer = ({ game, matchData, isPaused }) => {
    switch (game) {
        case "chess":
            return <ChessGame data={matchData} isPaused={isPaused} />;

        case "dota":
            return <div>Dota (TODO)</div>;

        case "csgo":
            return <div>CSGO (TODO)</div>;

        default:
            return <p>Game not supported</p>;
    }
};

export default GameRenderer;