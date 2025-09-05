import React, { useEffect, useState } from 'react';
import GameCard from '../components/HomePage/GameCard';
import Loading from '../components/Loading';
import useApiRequest from '../hooks/useApiRequest';

function HomePage() {
    const apiRequest = useApiRequest();
    const [games, setGames] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const receiveData = async () => {
            const games = await apiRequest('games', 'getAll', undefined, false, 'GET', false);
            setGames(games);
        };

        receiveData();
        setIsLoading(false);
    }, []);

    return (
        <section className="flex-1 p-6 flex flex-wrap justify-center items-center gap-6">
            {isLoading ? <Loading size={'small'} /> :
                <>
                    {games.map((game) => (
                        <GameCard
                            key={game.id}
                            data={game}
                        />
                    ))}
                </>}
        </section>
    );
}

export default HomePage;