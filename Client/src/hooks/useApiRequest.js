import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { fetchError } from '../constants/errorMessages';

function useApiRequest() {
    const navigate = useNavigate();
    const { updateToken } = useAuth();

    const apiUrl = 'https://localhost:7182/api';

    const apiRequest = async (
        controller,
        action,
        method = 'GET',
        isAuthenticated = false,
        credentials = false,
        values = null
    ) => {

        let token = localStorage.getItem('accessToken');

        const headers = {
            'Content-Type': 'application/json'
        };

        if (isAuthenticated && token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const requestOptions = {
            method,
            headers,
        };

        if (credentials) {
            requestOptions.credentials = 'include';
        }

        if (values) {
            requestOptions.body = JSON.stringify(values);
        }

        try {
            let response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);

            if (response.status === 401) {
                response = await handleUnauthorized(
                    `${apiUrl}/${controller}/${action}`,
                    requestOptions
                );
                console.log(response);
                if (!response) return;
            }

            if (response.status === 204) {
                return true;
            }

            if (response.status === 404) {
                navigate('/');
                return;
            }

            if (response.status >= 500) {
                throw new Error(fetchError);
            }

            const data = await response.json();

            if (data?.serverError) {
                throw new Error(data.serverError);
            }

            return data;

        } catch (error) {
            console.error(error);
            throw error;
        }
    };

    const refreshAccessToken = async () => {
        try {
            const response = await fetch(`${apiUrl}/account/refreshToken`, {
                method: 'GET',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            if (!response.ok) {
                return null;
            }

            const data = await response.json();
            return data.token;

        } catch (error) {
            console.error(error);
            return null;
        }
    };

    const handleUnauthorized = async (url, requestOptions) => {
        const newToken = await refreshAccessToken();

        if (!newToken) {
            navigate('/');
            return null;
        }

        updateToken(newToken);

        requestOptions.headers['Authorization'] = `Bearer ${newToken}`;

        return await fetch(url, requestOptions);
    };

    return apiRequest;
}

export default useApiRequest;