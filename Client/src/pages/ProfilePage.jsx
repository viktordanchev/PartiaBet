import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import ProfileSection from '../components/profilePage/ProfileSection';
import DepositSection from '../components/profilePage/DepositSection';
import WithdrawalSection from '../components/profilePage/WithdrawalSection';

function ProfilePage() {
    const navigate = useNavigate();
    const { tab } = useParams();

    const activeTab = tab || 'personalInfo';

    const renderSection = () => {
        switch (activeTab) {
            case 'personalInfo':
                return <ProfileSection />;
            case 'deposit':
                return <DepositSection />;
            case 'withdrawal':
                return <WithdrawalSection />;
            default:
                return null;
        }
    };

    const tabs = [
        { key: 'personalInfo', label: 'Personal info' },
        { key: 'deposit', label: 'Deposit' },
        { key: 'withdrawal', label: 'Withdrawal' },
    ];

    return (
        <section className="flex-1 p-6 flex flex-col">
            <div>
                {tabs.map((tab, index) => (
                    <button
                        key={tab.key}
                        onClick={() => navigate(`/profile/${tab.key}`)}
                        className={`px-5 py-2 text-lg transition font-medium
                            ${index === 0 && 'rounded-tl-xl'}
                            ${index === tabs.length - 1 && 'rounded-tr-xl'}
                            ${activeTab === tab.key
                                ? 'bg-gray-900/30 text-maincolor'
                                : 'bg-gray-900 text-gray-300 hover:cursor-pointer hover:bg-gray-900/30'
                            }`}
                    >
                        {tab.label}
                    </button>
                ))}
            </div>

            {renderSection()}
        </section>
    );
}

export default ProfilePage;