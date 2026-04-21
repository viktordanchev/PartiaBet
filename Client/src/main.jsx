import { BrowserRouter } from "react-router-dom";
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx';
import { LoadingProvider } from './contexts/LoadingContext';
import { NotificationsProvider } from './contexts/NotificationsContext';
import { AuthProvider } from './contexts/AuthContext';
import { AppHubProvider } from './contexts/AppHubContext';

createRoot(document.getElementById('root')).render(
    <LoadingProvider>
        <AuthProvider>
            <AppHubProvider>
                <BrowserRouter>
                    <NotificationsProvider>
                        <App />
                    </NotificationsProvider>
                </BrowserRouter>
            </AppHubProvider>
        </AuthProvider>
    </LoadingProvider>
)
