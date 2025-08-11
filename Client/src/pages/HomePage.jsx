import React from 'react';
import NavigationBar from '../components/homePage/NavigationBar';
import GamesList from '../components/homePage/GamesList';

function HomePage() {
    return (
        <section className="flex">
            <NavigationBar />
            <div className="border-l-2 border-maincolor mx-6"></div>
            <GamesList />
            <article></article>
        </section>
    );
}

export default HomePage;