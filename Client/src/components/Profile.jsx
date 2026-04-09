import React from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import ProfileImg from '../assets/images/profile-photo.jpg';

const Profile = ({ token }) => {
    const navigate = useNavigate();
    const decodedToken = jwtDecode(token);
    const userName = decodedToken['Username'];
    const imageUrl = decodedToken['ProfileImageUrl'];

    return (
        <div className="flex items-center gap-6">

            <button className="px-6 py-1 text-lg rounded-xl bg-gray-900 font-medium text-maincolor border-2 border-maincolor transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                onClick={() => navigate('/profile/deposit')}>
                Deposit
            </button>

            <div className="flex items-center gap-3">
                <img src={imageUrl ? imageUrl : ProfileImg}
                    className="w-13 rounded-full"
                />
                <div className="flex flex-col text-gray-300">
                    <p className="text-lg font-semibold">{userName}</p>
                    <div className="text-sm flex gap-1">
                        <p>Balance</p>
                        <p className="font-bold">0</p>
                    </div>
                </div>
            </div>

        </div>
    );
};

export default Profile;