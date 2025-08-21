import React from 'react';
import { Navigate, Route, Routes, useLocation } from 'react-router-dom';

import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import FriendsPage from './pages/FriendsPage';
import NotFoundPage from './pages/NotFoundPage';
import ChessGamePage from './pages/ChessGamePage';

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
                        <Route path="/games/chess" element={<ChessGamePage />} />

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