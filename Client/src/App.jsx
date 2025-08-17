import React from 'react';
import { Navigate, Route, Routes, useLocation } from 'react-router-dom';

import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';

import Footer from './components/Footer';
import Header from './components/Header';
import NavigationBar from './components/NavigationBar';

import { LoadingProvider } from './contexts/LoadingContext';

function App() {
    const location = useLocation();
    const hideNav = ["/login", "/register"].includes(location.pathname);

    return (
        <LoadingProvider>
            <Header />
            <main className="flex-grow flex">
                {!hideNav && <NavigationBar />}
                <Routes>
                    <Route path="/" element={<Navigate to="/home" />} />
                    <Route path="/home" element={<HomePage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                </Routes>
            </main>
            <Footer />
        </LoadingProvider>
    );
}

export default App;