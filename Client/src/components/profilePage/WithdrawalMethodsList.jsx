import React, { useState, useEffect } from 'react';
import { SiVisa, SiMastercard, SiRevolut } from 'react-icons/si';

const WithdrawalMethodsList = () => {
    const [amount, setAmount] = useState('');

    const handleWithdrawal = async () => {
        if (!amount || Number(amount) <= 0) return;

        console.log('Withdrawal:', {
            method: paymentMethod.id,
            amount: Number(amount),
        });
    };

    return (
        <article className="space-y-2">
            <h2 className="text-2xl font-semibold">Withdrawal Methods</h2>

            <div className="space-y-4">
                <div>
                    <h2 className="text-sm text-gray-400 mb-1">Payment method</h2>

                    <button className="flex items-center justify-center gap-3 px-6 py-4 rounded-xl text-3xl border border-gray-700 bg-gray-800 hover:bg-gray-700 hover:border-gray-600 hover:cursor-pointer">
                        <SiVisa />
                        <SiMastercard />
                        <SiRevolut />
                    </button>
                </div>

                <div>
                    <h2 className="text-sm text-gray-400 mb-1">Amount</h2>

                    <input type="number"
                        value={amount}
                        onChange={(e) => setAmount(e.target.value)}
                        placeholder="Enter amount"
                        className="w-full p-3 rounded-xl bg-gray-800 border border-gray-700 outline-none focus:border-gray-600 [&::-webkit-outer-spin-button]:appearance-none
                        [&::-webkit-inner-spin-button]:appearance-none
                        [-moz-appearance:textfield]"
                    />
                </div>

                <button className="px-6 py-2 text-lg rounded-xl bg-green-600 font-medium transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105"
                    onClick={handleWithdrawal}>
                    Withdraw
                </button>
            </div>
        </article>
    );
};

export default WithdrawalMethodsList;