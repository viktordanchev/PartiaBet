import React from 'react';
import NavigationBar from '../components/homePage/NavigationBar';
import GamesList from '../components/homePage/GamesList';

function HomePage() {
    return (
        <section className="flex-grow flex">
            <NavigationBar />
            <GamesList />
        </section>
    );
}

export default HomePage;