import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { fetchError } from '../constants/errorMessages';

function useApiRequest() {
    const navigate = useNavigate();
    const { updateToken } = useAuth();

    const apiUrl = 'https://localhost:7182/api';
    const headers = { 'Content-Type': 'application/json' };

    const apiRequest = async (controller, action, values, isAuthenticated, method = 'GET', credentials = false) => {
        const requestOptions = {
            method: `${method}`,
            headers: headers
        };

        if (isAuthenticated) {
            headers['Authorization'] = `Bearer ${sessionStorage.getItem('accessToken')}`;
        }

        if (credentials) {
            requestOptions.credentials = 'include';
        }

        if (values) {
            requestOptions.body = JSON.stringify(values);
        }

        try {
            await new Promise(resolve => setTimeout(resolve, 3300));
            let response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);

            if (response.status === 401) {
                const newToken = await refreshAccessToken();
                updateToken(newToken);

                if (newToken) {
                    headers["Authorization"] = `Bearer ${newToken}`;

                    response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);
                } else {
                    navigate('/friends');
                    return;
                }
            } else if (response.status >= 500) {
                throw new Error(fetchError);
            }

            const data = await response.json();

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