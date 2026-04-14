import React, { useState, useEffect } from 'react';
import useApiRequest from '../../hooks/useApiRequest';
import DepositHistoryList from './DepositHistoryList';
import DepositMethodsList from './DepositMethodsList';

const DepositSection = () => {
    const apiRequest = useApiRequest();

    useEffect(() => {
        document.title = 'Deposit';
    }, []);

    return (
        <section className="flex-1 p-6 bg-gray-900 rounded-b-xl rounded-tr-xl text-white">

            <DepositMethodsList />
            <div className="my-16 border-t border-gray-700" />
            <DepositHistoryList />

        </section>
    );
};

export default DepositSection;