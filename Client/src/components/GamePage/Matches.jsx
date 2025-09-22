import React, { useState, useEffect } from 'react';
import MatchList from './Matches/MatchList';
import useApiRequest from '../../hooks/useApiRequest';
import Loading from '../Loading';

const Matches = ({ game }) => {
    const apiRequest = useApiRequest();
    const [matches, setMatches] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    
    useEffect(() => {
        const receiveData = async () => {
            const matches = await apiRequest('matches', 'getActiveMatches', 'POST', false, false, game);

            setIsLoading(false);
            setMatches(matches);
        };

        receiveData();
    });

    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 space-y-6">
            <h2 className="text-gray-300 text-3xl font-semibold text-center">
                Tables
            </h2>
            {isLoading ? <Loading size={'small'} /> :
                <section className="space-y-6">
                    <MatchList isCasualGame={false} matches={matches} />
                    <MatchList isCasualGame={true} matches={matches} />
                </section>}
        </article>
    );
};

export default Matches;