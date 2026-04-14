import React, { useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from '@fortawesome/free-solid-svg-icons';

const WithdrawalHistoryList = () => {
    const [withdrawals, setWithdrawals] = useState([]);

    useEffect(() => {
        const mockData = [
            {
                id: 1,
                date: '10.04.2026 16:42',
                amount: 100,
                method: 'Credit Card',
                status: 'Approved',
            },
            {
                id: 2,
                date: '2026-04-11',
                amount: 50,
                method: 'PayPal',
                status: 'Pending',
            },
        ];

        setWithdrawals(mockData);
    }, []);

    const getStatusColor = (status) => {
        switch (status) {
            case 'Approved':
                return 'text-green-400';
            case 'Cancelled':
                return 'text-red-400';
            default:
                return '';
        }
    };

    return (
        <section className="space-y-2">
            <h2 className="text-2xl font-semibold">Withdrawal History</h2>

            <table className="min-w-full text-center bg-gray-800 rounded-xl overflow-hidden border border-gray-600 [&_th]:border [&_td]:border [&_th]:border-gray-600 [&_td]:border-gray-600">
                <thead>
                    <tr className="bg-gray-700 text-xl">
                        <th className="p-3">Date</th>
                        <th className="p-3">Amount</th>
                        <th className="p-3">Method</th>
                        <th className="p-3">Status</th>
                        <th className="p-3">Cancel</th>
                    </tr>
                </thead>

                <tbody>
                    {withdrawals.length === 0 ? (
                        <tr>
                            <td colSpan="4" className="p-3 text-center">
                                No withdrawals found
                            </td>
                        </tr>
                    ) : (
                        withdrawals.map((withdrawal) => (
                            <tr key={withdrawal.id}>
                                <td className="p-3">{withdrawal.date}</td>
                                <td className="p-3">${withdrawal.amount}</td>
                                <td className="p-3">{withdrawal.method}</td>
                                <td className={`p-3 ${getStatusColor(withdrawal.status)}`}>
                                    {withdrawal.status}
                                </td>
                                {withdrawal.status === 'Pending' && <td className="text-red-500 text-2xl hover:cursor-pointer"><FontAwesomeIcon icon={faXmark} /></td>}
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
        </section>
    );
};

export default WithdrawalHistoryList;