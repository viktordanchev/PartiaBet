import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import Board from './Board';
import PlayerCard from '../PlayerCard';
import TurnTimer from './TurnTimer';
import { useMatchHub } from '../../../contexts/MatchHubContext';

const ChessMatch = ({ data, isPaused }) => {
    const { newMove } = useMatchHub();
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const [opponent, setOpponent] = useState(data.players.find(p => p.id !== userId));
    const [loggedPlayer, setLoggedPlayer] = useState(data.players.find(p => p.id === userId));
    const [board, setBoard] = useState(data.board);
    const [playerInTurn, setPlayerInTurn] = useState(data.players.find(p => p.isOnTurn)?.id);
    console.log(data);
    useEffect(() => {
        if (!newMove) return;

        const { gameBoard, newPlayerId, duration } = newMove;
        
        setBoard(gameBoard);

        if (newPlayerId === loggedPlayer.id) {
            setLoggedPlayer(prev => ({
                ...prev,
                turnTimeLeft: duration
            }));
        } else {
            setOpponent(prev => ({
                ...prev,
                turnTimeLeft: duration
            }));
        }

        setPlayerInTurn(newPlayerId);
    }, [newMove]);

    return (
        <section className="flex gap-3">
            <Board
                data={board}
                isMyTurn={playerInTurn === loggedPlayer.id}
            />
            <article className="flex flex-col justify-between text-gray-300">
                <div className="space-y-3">
                    <PlayerCard data={opponent} />
                    <TurnTimer
                        timeLeft={opponent.turnTimeLeft}
                        isActive={playerInTurn === opponent.id}
                        isPaused={isPaused}
                    />
                </div>
                <div className="space-y-3">
                    <PlayerCard data={loggedPlayer} />
                    <TurnTimer
                        timeLeft={loggedPlayer.turnTimeLeft}
                        isActive={playerInTurn === loggedPlayer.id}
                        isPaused={isPaused}
                    />
                </div>
            </article>
        </section>
    );
};

export default ChessMatch;