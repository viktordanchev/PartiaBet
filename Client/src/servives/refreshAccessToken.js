import apiRequest from './apiRequest';

async function refreshAccessToken() {
    const response = await apiRequest(
        'account',
        'refreshToken',
        undefined,
        false,
        'GET',
        true
    );

    return response.token;
}

export default refreshAccessToken;
