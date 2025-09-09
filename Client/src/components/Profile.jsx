import React from 'react';
import { jwtDecode } from 'jwt-decode';
import ProfileImg from '../assets/images/profile-photo.jpg';

const Profile = (token) => {
    const decodedToken = jwtDecode(token);

    const userName = decodedToken['Username'];
    const imageUrl = decodedToken['ProfileImageUrl'];

    return (
        <div>
            <p>{userName}</p>
            <img src={userName ? userName : ProfileImg} />
        </div>
    );
};

export default Profile;