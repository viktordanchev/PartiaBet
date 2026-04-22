import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useSignalREvent } from "../../../hooks/signalR/useSignalREvent";

import Board from './Board';
import PlayerCard from '../PlayerCard';
import TurnTimer from './TurnTimer';

const ChessGame = ({ data, isPaused }) => {
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    const [board, setBoard] = useState(data.board);
    const [players, setPlayers] = useState(() => {
        const map = {};
        data.players.forEach(p => {
            map[p.id] = { ...p };
        });
        return map;
    });

    const [playerInTurn, setPlayerInTurn] = useState(
        data.players.find(p => p.isOnTurn)?.id
    );

    const loggedPlayer = players[userId];
    const opponent = Object.values(players).find(p => p.id !== userId);

    useSignalREvent("ReceiveMove", (gameBoard, nextPlayerId, duration) => {
        setBoard(gameBoard);

        setPlayers(prev => ({
            ...prev,
            [nextPlayerId]: {
                ...prev[nextPlayerId],
                turnTimeLeft: duration
            }
        }));

        setPlayerInTurn(nextPlayerId);
    });

    return (
        <section className="flex gap-3">
            <Board
                data={board}
                isMyTurn={playerInTurn === userId}
            />

            <article className="flex flex-col justify-between text-gray-300">
                <div className="space-y-3">
                    <PlayerCard data={opponent} />
                    <TurnTimer
                        key={opponent.id}
                        timeLeft={opponent.turnTimeLeft}
                        isActive={playerInTurn === opponent.id}
                        isPaused={isPaused}
                    />
                </div>

                <div className="space-y-3">
                    <PlayerCard data={loggedPlayer} />
                    <TurnTimer
                        key={loggedPlayer.id}
                        timeLeft={loggedPlayer.turnTimeLeft}
                        isActive={playerInTurn === loggedPlayer.id}
                        isPaused={isPaused}
                    />
                </div>
            </article>
        </section>
    );
};

export default ChessGame;