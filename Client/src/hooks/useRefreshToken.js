import apiRequest from '../servives/apiRequest';

function useRefreshToken() {
    const refreshAccessToken = async () => {
        const response = await apiRequest('account', 'refreshToken', undefined, false, 'GET', true);

        return response.token;
    };

    return { refreshAccessToken };
}

export default useRefreshToken;