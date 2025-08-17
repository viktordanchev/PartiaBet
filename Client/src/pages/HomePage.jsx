import React from 'react';
import GamesList from '../components/homePage/GamesList';

function HomePage() {
    return (
        <section className="flex-grow flex">
            <GamesList />
        </section>
    );
}

export default HomePage;