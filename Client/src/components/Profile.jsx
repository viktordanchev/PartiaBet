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
            <p className="text-gray-300 text-xl font-semibold">{userName}</p>
        </div>
    );
};

export default Profile;