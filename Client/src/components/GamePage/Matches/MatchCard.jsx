import React from 'react';
import { useNavigate } from "react-router-dom";
import { useMatchHub } from '../../../contexts/MatchHubContext';
import { useLoading } from '../../../contexts/LoadingContext';
import PlayerCard from './PlayerCard';

const MatchCard = ({ match }) => {
    const navigate = useNavigate();
    const { connection } = useMatchHub();
    const { setIsLoading } = useLoading();
    const isMatchFull = false;
    
    const joinMatch = async () => {
        setIsLoading(true);
        await connection.invoke("JoinMatch", match.id.toString());
        setIsLoading(false);

        navigate(`/games/chess/match/${match.id}`);
    };

    return (
        <li className="w-full p-2 flex flex-col items-center gap-3 rounded-xl border border-gray-700 bg-gray-900">
            <div className="w-full flex flex-col text-center gap-1">
                <p>{match.players.length}/{match.maxPlayersCount}</p>
                <div className="flex justify-center text-xs">
                    {match.players.map((player) =>
                        <PlayerCard
                            key={player.id}
                            data={player}
                        />
                    )}
                    {Array.from({ length: match.maxPlayersCount - match.players.length }).map((_, index) => (
                        <PlayerCard
                            key={index}
                            data={null}
                        />
                    ))}
                </div>
            </div>
            <button className={`py-1 px-3 rounded-xl text-white font-medium transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105 ${isMatchFull ? 'bg-red-600' : 'bg-green-600'}`}
                onClick={joinMatch}>
                {isMatchFull ? (
                    <>
                        <span className="inline-block w-2 h-2 rounded-full bg-white mr-1"></span>
                        <span>Live</span>
                    </>
                ) : (
                    <>
                            {match.betAmount === 0 ? 'Play' : `Bet ${match.betAmount}$`}
                    </>
                )}
            </button>
        </li>
    );
};

export default MatchCard;