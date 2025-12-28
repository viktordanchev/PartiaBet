import React, { useState, useEffect } from 'react';
import MatchList from './Matches/MatchList';
import useApiRequest from '../../hooks/useApiRequest';
import Loading from '../Loading';
import { useHub } from '../../contexts/HubContext';

const Matches = ({ gameId }) => {
    const { connection } = useHub();
    const apiRequest = useApiRequest();
    const [matches, setMatches] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const casualMatches = matches.filter(m => m.betAmount === 0);
    const betMatches = matches.filter(m => m.betAmount > 0);
    
    useEffect(() => {
        const receiveData = async () => {
            const matches = await apiRequest('matches', 'getActiveMatches', 'POST', false, false, gameId);
            
            setIsLoading(false);
            setMatches(matches);
        };

        receiveData();
    }, []);

    useEffect(() => {
        if (!connection) return;

        const handleReceiveMatch = (match) => {
            setMatches(prev => [match, ...prev]);
        };

        connection.on("ReceiveMatch", handleReceiveMatch);

        return () => {
            connection.off("ReceiveMatch", handleReceiveMatch);
        };
    }, [connection]);

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