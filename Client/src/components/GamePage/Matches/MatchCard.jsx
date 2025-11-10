import React from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { useHub } from '../../../contexts/HubContext';
import { useLoading } from '../../../contexts/LoadingContext';
import PlayerCard from './PlayerCard';

const MatchCard = ({ match, game }) => {
    const navigate = useNavigate();
    const { connection } = useHub();
    const { setIsLoading } = useLoading();
    const isMatchFull = false;
    
    const joinMatch = async () => {
        const token = localStorage.getItem('accessToken');
        var playerData = {
            id: jwtDecode(token)['Id'],
            username: jwtDecode(token)['Username'],
            profileImageUrl: jwtDecode(token)['ProfileImageUrl'],
        };

        setIsLoading(true);
        await connection.invoke("JoinMatch", game, match.id.toString(), playerData);
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