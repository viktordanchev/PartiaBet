import { BrowserRouter } from "react-router-dom";
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx';
import { HubProvider } from './contexts/HubContext';
import { LoadingProvider } from './contexts/LoadingContext';

createRoot(document.getElementById('root')).render(
    <BrowserRouter>
        <HubProvider>
            <LoadingProvider>
                <App />
            </LoadingProvider>
        </HubProvider>
    </BrowserRouter>
)
