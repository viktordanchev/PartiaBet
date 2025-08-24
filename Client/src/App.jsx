import React from 'react';
import { Route, Routes, useLocation } from 'react-router-dom';

import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import FriendsPage from './pages/FriendsPage';
import NotFoundPage from './pages/NotFoundPage';
import GamePage from './pages/GamePage';

import Footer from './components/Footer';
import Header from './components/Header';
import NavigationBar from './components/NavigationBar';

import { LoadingProvider } from './contexts/LoadingContext';

function App() {
    const location = useLocation();
    const hideNav = ['/login', '/register'].includes(location.pathname);

    return (
        <LoadingProvider>
            <Header />
            <main className="flex-grow flex">
                {!hideNav && <NavigationBar />}
                <div className="flex-1 flex justify-center p-6">
                    <Routes>
                        <Route path="*" element={<NotFoundPage />} />

                        <Route path="/" element={<HomePage />} />
                        <Route path="/friends" element={<FriendsPage />} />
                        <Route path="/games/chess" element={<GamePage game={'Chess'} />} />
                        <Route path="/games/backgammon" element={<GamePage game={'Backgammon'} />} />
                        <Route path="/games/belote" element={<GamePage game={'Belote'} />} />
                        <Route path="/games/sixty-six" element={<GamePage game={'Sixty-Six'} />} />

                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/register" element={<RegisterPage />} />
                    </Routes>
                </div>
            </main>
            <Footer />
        </LoadingProvider>
    );
}

export default App;