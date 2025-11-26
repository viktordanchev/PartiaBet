import React from 'react';
import { Route, Routes, useLocation, useMatch } from 'react-router-dom';

import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import FriendsPage from './pages/FriendsPage';
import NotFoundPage from './pages/NotFoundPage';
import GamePage from './pages/GamePage';
import MatchPage from './pages/MatchPage';

import Footer from './components/Footer';
import Header from './components/Header';
import NavigationBar from './components/NavigationBar';
import SessionEndNotification from './components/SessionEndNotification';

function App() {
    const location = useLocation();
    const matchGame = useMatch('/games/:game/match/:matchId');
    const pagesNoNav = ['', '/login', '/register', matchGame?.pathname].includes(location.pathname);

    return (
        <>
            {!pagesNoNav && <NavigationBar />}
            <div className={`flex-grow flex flex-col ${!pagesNoNav && "ml-80"}`}>
                <Header isLogoVis={pagesNoNav} />
                <main className="flex-grow flex">
                    <Routes>
                        <Route path="*" element={<NotFoundPage />} />

                        <Route path="/" element={<HomePage />} />
                        <Route path="/friends" element={<FriendsPage />} />
                        <Route path="/games/:game" element={<GamePage />} />
                        <Route path="/games/:game/match/:matchId" element={<MatchPage />} />

                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/register" element={<RegisterPage />} />
                    </Routes>
                </main>
                <Footer />
            </div>
            <SessionEndNotification />
        </>
    );
}

export default App;