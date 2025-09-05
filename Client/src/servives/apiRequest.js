import { fetchError } from '../constants/errorMessages';
import refreshAccessToken from './refreshAccessToken';

const apiUrl = 'https://localhost:7182/api';
const headers = { 'Content-Type': 'application/json' };

async function apiRequest(controller, action, values, isAuthenticated, method, credentials) {
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
        let response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);

        if (response.status === 401) {
            const newToken = await refreshAccessToken();

            if (newToken) {
                headers["Authorization"] = `Bearer ${newToken}`;

                response = await fetch(`${apiUrl}/${controller}/${action}`, requestOptions);
            } else {
                clearToken();
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
}

export default apiRequest;