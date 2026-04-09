import React, { useState, useEffect } from 'react';
import ProfileSection from '../components/profilePage/ProfileSection';
import DepositSection from '../components/profilePage/DepositSection';
import WithdrawalSection from '../components/profilePage/WithdrawalSection';

function ProfilePage() {
    const [activeTab, setActiveTab] = useState('profile');

    useEffect(() => {
        document.title = 'Profile';
    }, []);

    const renderSection = () => {
        switch (activeTab) {
            case 'profile':
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
        { key: 'profile', label: 'Profile' },
        { key: 'deposit', label: 'Deposit' },
        { key: 'withdrawal', label: 'Withdrawal' },
    ];

    return (
        <section className="flex-1 p-6 flex flex-col">

            <div>
                {tabs.map((tab, index) => (
                    <button key={tab.key}
                        onClick={() => setActiveTab(tab.key)}
                        className={`px-5 py-2 text-lg transition font-medium
                            ${index === 0 && 'rounded-tl-xl'}
                            ${index === tabs.length - 1 && 'rounded-tr-xl'}
                            ${activeTab === tab.key
                            ? 'bg-gray-900/30 text-maincolor'
                                : 'bg-gray-900 text-gray-300 hover:bg-gray-900/30 hover:cursor-pointer'
                            }`}>
                        {tab.label}
                    </button>
                ))}
            </div>

            <article className="flex-1 p-6 bg-gray-900 rounded-b-xl rounded-tr-xl text-white">
                {renderSection()}
            </article>

        </section>
    );
}

export default ProfilePage;