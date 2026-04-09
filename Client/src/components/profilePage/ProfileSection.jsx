import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';

import ProfileImage from '../../assets/images/profile-photo.jpg';

const ProfileSection = () => {
    const apiRequest = useApiRequest();

    return (
        <>

            <div className="flex flex-col">
                <img className="h-35 w-35 m-auto rounded-full shadow-lg shadow-gray-900"
                    src={ProfileImage} />

                <p className="text-gray-300 text-3xl text-center font-semibold">
                    username
                </p>
            </div>

        </>
    );
};

export default ProfileSection;