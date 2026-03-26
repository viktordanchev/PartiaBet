import { BrowserRouter } from "react-router-dom";
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx';
import { LoadingProvider } from './contexts/LoadingContext';
import { NotificationsProvider } from './contexts/NotificationsContext';
import { AuthProvider } from './contexts/AuthContext';
import { MatchHubProvider } from './contexts/MatchHubContext';
import { PresenceHubProvider } from './contexts/PresenceHubContext';

createRoot(document.getElementById('root')).render(
    <LoadingProvider>
        <AuthProvider>
            <MatchHubProvider>
                <PresenceHubProvider>
                    <BrowserRouter>
                        <NotificationsProvider>
                            <App />
                        </NotificationsProvider>
                    </BrowserRouter>
                </PresenceHubProvider>
            </MatchHubProvider>
        </AuthProvider>
    </LoadingProvider>
)
