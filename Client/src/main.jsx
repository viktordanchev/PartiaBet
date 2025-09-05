import { BrowserRouter } from "react-router-dom";
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx';
import { HubProvider } from './contexts/HubContext';
import { LoadingProvider } from './contexts/LoadingContext';
import { NotificationsProvider } from './contexts/NotificationsContext';
import { AuthProvider } from './contexts/AuthContext';

createRoot(document.getElementById('root')).render(
    <BrowserRouter>
        <AuthProvider>
            <HubProvider>
                <LoadingProvider>
                    <NotificationsProvider>
                        <App />
                    </NotificationsProvider>
                </LoadingProvider>
            </HubProvider>
        </AuthProvider>
    </BrowserRouter>
)
