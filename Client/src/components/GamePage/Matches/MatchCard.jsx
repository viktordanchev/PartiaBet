import React from 'react';
import PlayerCard from './PlayerCard';

const MatchCard = ({ data }) => {
    const isMatchFull = false;

    const teams = [
        ...data.teams,
        ...Array.from({ length: data.teamsCount - data.teams.length }, () => ({
            id: null,
            players: [],
        })),
    ].map((team, teamIndex) => {
        const playerCount = team.players.length ?? 0;

        const players = [
            ...team.players,
            ...Array.from({ length: data.teamSize - playerCount }, () => ({})),
        ];

        return {
            ...team,
            players,
            id: team.id ?? `empty-team-${teamIndex}`,
        };
    });
    
    return (
        <li className="w-full p-2 flex flex-col items-center gap-3 rounded-xl border border-gray-700 bg-gray-900">
            <div className="w-full flex items-center text-xs text-center">
                {teams.map((team, teamIndex) =>
                    <>
                        <div key={team.id} className="flex-1 flex justify-center items-center gap-2">
                            {team.players.map((player) => (
                                <PlayerCard data={player} />
                            ))}
                        </div>
                        {teamIndex < teams.length - 1 && (
                            <p className="flex-none text-2xl font-semibold mx-2">VS</p>
                        )}
                    </>
                )}
            </div>
            <button className={`py-1 px-3 rounded-xl text-white font-medium transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105 ${isMatchFull ? 'bg-red-600' : 'bg-green-600'}`}>
                {isMatchFull ? (
                    <>
                        <span className="inline-block w-2 h-2 rounded-full bg-white mr-1"></span>
                        <span>Live</span>
                    </>
                ) : (
                    <>
                        {data.betAmount === 0 ? 'Play' : `Bet ${data.betAmount}$`}
                    </>
                )}
            </button>
        </li>
    );
};

export default MatchCard;