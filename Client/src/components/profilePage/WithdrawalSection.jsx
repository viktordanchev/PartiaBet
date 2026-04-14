import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';

import ProfileImage from '../../assets/images/profile-photo.jpg';
import WithdrawalHistoryList from './WithdrawalHistoryList';
import WithdrawalMethodsList from './WithdrawalMethodsList';

const WithdrawalSection = () => {
    const apiRequest = useApiRequest();

    useEffect(() => {
        document.title = 'Withdrawal';
    });

    return (
        <section className="flex-1 p-6 bg-gray-900 rounded-b-xl rounded-tr-xl text-white">

            <WithdrawalMethodsList />
            <div className="my-16 border-t border-gray-700" />
            <WithdrawalHistoryList />

        </section>
    );
};

export default WithdrawalSection;