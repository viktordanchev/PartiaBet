import React, { useState, useEffect } from 'react';
import MatchList from './matches/MatchList';
import useApiRequest from '../../hooks/useApiRequest';
import Loading from '../Loading';
import { useSignalREvent } from "../../hooks/signalR/useSignalREvent";

const Matches = ({ gameType }) => {
    const apiRequest = useApiRequest();
    const [matches, setMatches] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const casualMatches = matches.filter(m => m.betAmount === 0);
    const betMatches = matches.filter(m => m.betAmount > 0);

    useEffect(() => {
        const receiveData = async () => {
            const matches = await apiRequest('matches', 'getActiveMatches', 'POST', false, false, gameType);

            setIsLoading(false);
            setMatches(matches);
        };

        receiveData();
    }, []);

    useSignalREvent("ReceiveMatch", (newMatch) => {
        setMatches(prevMatches =>
            [newMatch, ...prevMatches]
        );
    });

    useSignalREvent("ReceivePlayer", (matchId, player) => {
        setMatches(prevMatches =>
            prevMatches.map(match => {
                if (match.id !== matchId) return match;

                return {
                    ...match,
                    players: [...match.players, player]
                };
            })
        );
    });

    useSignalREvent("RemovePlayer", (matchId, playerId) => {
        setMatches(prevMatches =>
            prevMatches.map(match => {
                if (match.id !== matchId) return match;

                return {
                    ...match,
                    players: match.players.filter(p => p.id !== playerId)
                };
            })
        );
    });

    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 space-y-6">
            <h2 className="text-gray-300 text-3xl font-semibold text-center">
                Tables
            </h2>
            {isLoading ? <Loading size={'small'} /> :
                <section className="space-y-6">
                    <MatchList isCasualGame={false} data={betMatches} />
                    <MatchList isCasualGame={true} data={casualMatches} />
                </section>}
        </article>
    );
};

export default Matches;