import React from 'react';
import { jwtDecode } from 'jwt-decode';
import ProfileImg from '../assets/images/profile-photo.jpg';

const Profile = ({ token }) => {
    const decodedToken = jwtDecode(token);
    const userName = decodedToken['Username'];
    const imageUrl = decodedToken['ProfileImageUrl'];

    return (
        <div className="flex items-center gap-3">
            <img src={imageUrl ? imageUrl : ProfileImg}
                className="w-13 rounded-full"
            />
            <div className="flex flex-col text-gray-300">
                <p className="text-xl font-semibold">{userName}</p>
                <div className="text-xs flex gap-1">
                    <p>Balance</p>
                    <p className="font-bold">0</p>
                </div>
            </div>
        </div>
    );
};

export default Profile;