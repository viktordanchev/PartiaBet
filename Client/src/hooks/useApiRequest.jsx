import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { fetchError } from '../constants/errorMessages';

function useApiRequest() {
    const navigate = useNavigate();
    const { updateToken, token } = useAuth();

    const apiUrl = 'https://localhost:7182/api';
    const headers = { 'Content-Type': 'application/json' };

    const apiRequest = async (controller, action, method = 'GET', isAuthenticated, credentials = false, values) => {
        const requestOptions = {
            method: `${method}`,
            headers: headers
        };

        if (isAuthenticated) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        if (credentials) {
            requestOptions.credentials = 'include';
        }

        if (values) {
            requestOptions.body = JSON.stringify(values);
        }
        
        try {
            let response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);
            
            if (response.status === 204) {
                return true;
            } else if (response.status === 401) {
                const newToken = await refreshAccessToken();
                updateToken(newToken);

                if (newToken) {
                    headers["Authorization"] = `Bearer ${newToken}`;

                    response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);
                } else {
                    navigate('/');
                    return;
                }
            } else if (response.status >= 500) {
                throw new Error(fetchError);
            }

            const data = await response?.json();

            if (data.serverError) {
                throw new Error(data.serverError);
            }

            return data;
        } catch (error) {
            console.error(error);
        }
    };

    const refreshAccessToken = async () => {
        const response = await fetch(`${apiUrl}/account/refreshToken`, {
            method: 'GET',
            credentials: 'include',
            headers: headers
        });

        const data = await response.json();

        return data.token;
    };

    return apiRequest;
}

export default useApiRequest;