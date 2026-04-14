import React, { useState, useEffect } from 'react';

const DepositHistoryList = () => {
    const [deposits, setDeposits] = useState([]);

    useEffect(() => {
        const mockData = [
            {
                id: 1,
                date: '10.04.2026 16:42',
                amount: 100,
                method: 'Credit Card',
                status: 'Success',
            },
            {
                id: 2,
                date: '2026-04-11',
                amount: 50,
                method: 'PayPal',
                status: 'Failed',
            },
        ];

        setDeposits(mockData);
    }, []);

    return (
        <article className="space-y-2">
            <h2 className="text-2xl font-semibold">Deposit History</h2>

            <table className="min-w-full text-center bg-gray-800 rounded-xl overflow-hidden border border-gray-600 [&_th]:border [&_td]:border [&_th]:border-gray-600 [&_td]:border-gray-600">
                <thead>
                    <tr className="bg-gray-700 text-xl">
                        <th className="p-3">Date</th>
                        <th className="p-3">Amount</th>
                        <th className="p-3">Method</th>
                        <th className="p-3">Status</th>
                    </tr>
                </thead>

                <tbody>
                    {deposits.length === 0 ? (
                        <tr>
                            <td colSpan="4" className="p-3 text-center">
                                No deposits found
                            </td>
                        </tr>
                    ) : (
                        deposits.map((deposit) => (
                            <tr key={deposit.id}>
                                <td className="p-3">{deposit.date}</td>
                                <td className="p-3">${deposit.amount}</td>
                                <td className="p-3">{deposit.method}</td>
                                <td className={`p-3 ${deposit.status === 'Success' ? 'text-green-400' : 'text-red-400'}`}>
                                    {deposit.status}
                                </td>
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
        </article>
    );
};

export default DepositHistoryList;