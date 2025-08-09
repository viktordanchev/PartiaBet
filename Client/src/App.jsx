import React from 'react';
import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom';

import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';

import Footer from './components/Footer';
import Header from './components/Header';

function App() {
    return (
        <BrowserRouter>
            <Header />
            <main>
                <Routes>
                    <Route path="/" element={<Navigate to="/home" />} />
                    <Route path="/home" element={<HomePage />} />
                    <Route path="/login" element={<LoginPage />} />
                </Routes>
            </main>
            <Footer />
        </BrowserRouter>
    );
}

export default App;