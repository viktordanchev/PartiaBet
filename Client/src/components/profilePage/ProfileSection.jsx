import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';

import ProfileImage from '../../assets/images/profile-photo.jpg';

const ProfileSection = () => {
    const apiRequest = useApiRequest();

    useEffect(() => {
        document.title = 'Personal info';
    });

    return (
        <article className="flex-1 p-6 bg-gray-900 rounded-b-xl rounded-tr-xl text-white flex flex-col items-center justify-between">

            <div className="flex flex-col">
                <img className="h-35 w-35 m-auto rounded-full shadow-lg shadow-gray-900"
                    src={ProfileImage} />

                <p className="text-gray-300 text-3xl text-center font-semibold">
                    username
                </p>
            </div>

            <button className="px-6 py-2 text-xl rounded-xl bg-maincolor font-medium text-gray-900 transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                Save
            </button>

        </article>
    );
};

export default ProfileSection;