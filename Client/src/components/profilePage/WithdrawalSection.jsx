import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';

import ProfileImage from '../../assets/images/profile-photo.jpg';

const WithdrawalSection = () => {
    const apiRequest = useApiRequest();

    useEffect(() => {
        document.title = 'Withdrawal';
    });

    return (
        <article className="flex-1 p-6 bg-gray-900 rounded-b-xl rounded-tr-xl text-white">

            <div className="flex flex-col">
                <img className="h-35 w-35 m-auto rounded-full shadow-lg shadow-gray-900"
                    src={ProfileImage} />

                <p className="text-gray-300 text-3xl text-center font-semibold">
                    username
                </p>
            </div>

        </article>
    );
};

export default WithdrawalSection;