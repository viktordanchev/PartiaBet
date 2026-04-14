import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';

import DepositHistoryList from './DepositHistoryList';

const DepositSection = () => {
    const apiRequest = useApiRequest();

    useEffect(() => {
        document.title = 'Deposit';
    });

    return (
        <section className="flex-1 p-6 bg-gray-900 rounded-b-xl rounded-tr-xl text-white">
            <DepositHistoryList />
        </section>
    );
};

export default DepositSection;