import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';

import ProfileImage from '../../assets/images/profile-photo.jpg';

const PlayerInfo = ({ playerData }) => {
    const apiRequest = useApiRequest();
    const [friendshipStatus, setFriendshipStatus] = useState(playerData.friendshipStatus);
    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];
    
    const handleAddFriend = async () => {
        await apiRequest('friends', 'sendFriendRequest', 'POST', true, false, playerData.id);
        setFriendshipStatus('Pending');
    };

    const handleRemoveFriend = async () => {
        await apiRequest('friends', 'removeFriend', 'POST', true, false, playerData.id);
        setFriendshipStatus('None');
    };

    const handleCancelFriendRequest = async () => {
        await apiRequest('friends', 'cancelFriendRequest', 'POST', true, false, playerData.id);
        setFriendshipStatus('None');
    };

    const handleAcceptFriendRequest = async () => {
        await apiRequest('friends', 'acceptFriendRequest', 'POST', true, false, playerData.id);
        setFriendshipStatus('Accepted');
    };

    return (
        <article className="w-full flex flex-col gap-6 items-center bg-gray-800 p-6 rounded border border-gray-500 shadow-xl shadow-gray-900">

            <div className="flex flex-col">
                <img className="h-35 w-35 m-auto rounded-full shadow-lg shadow-gray-900"
                    src={playerData.profileImageUrl ? playerData.profileImageUrl : ProfileImage} />

                <p className="text-gray-300 text-3xl text-center font-semibold">
                    {playerData.username}
                </p>
            </div>

            {friendshipStatus == 'None' && (
                <button className="px-3 py-2 rounded-xl bg-green-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                    onClick={handleAddFriend}>
                    Add Friend
                </button>
            )}

            {friendshipStatus == 'Accepted' && (
                <button className="px-3 py-2 rounded-xl bg-red-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                    onClick={handleRemoveFriend}>
                    Remove Friend
                </button>
            )}

            <div className="flex gap-6">
                {(friendshipStatus == 'Pending' && playerData.friendshipRequesterId && playerData.friendshipRequesterId != userId) && (
                    <button className="px-3 py-2 rounded-xl bg-green-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                        onClick={handleAcceptFriendRequest}>
                        Accept
                    </button>
                )}

                {(friendshipStatus == 'Pending') && (
                    <button className="px-3 py-2 rounded-xl bg-red-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                        onClick={handleCancelFriendRequest}>
                        Cancel
                    </button>
                )}
            </div>
        </article>
    );
};

export default PlayerInfo;