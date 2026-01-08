import { BrowserRouter } from "react-router-dom";
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx';
import { LoadingProvider } from './contexts/LoadingContext';
import { NotificationsProvider } from './contexts/NotificationsContext';
import { AuthProvider } from './contexts/AuthContext';

createRoot(document.getElementById('root')).render(
    <AuthProvider>
        <BrowserRouter>
                <LoadingProvider>
                    <NotificationsProvider>
                        <App />
                    </NotificationsProvider>
                </LoadingProvider>
        </BrowserRouter>
    </AuthProvider>
)
