import React, { useState, useEffect } from 'react';
import ProfileImage from '../assets/images/profile-photo.jpg';
import { usePresenceHub } from '../contexts/PresenceHubContext';

function FriendStatusAlert() {
    const { userStatus } = usePresenceHub();
    const [visible, setVisible] = useState(false);
    const [leaving, setLeaving] = useState(false);

    useEffect(() => {
        if (userStatus) {
            setVisible(true);
            setLeaving(false);

            const timer = setTimeout(() => {
                setLeaving(true);

                setTimeout(() => {
                    setVisible(false);
                }, 400);
            }, 3000);

            return () => clearTimeout(timer);
        }
    }, [userStatus]);

    if (!userStatus || !visible) return null;

    return (
        <article className={`h-25 w-70 bg-gray-900 rounded-xl border border-gray-500 shadow-xl shadow-gray-900 p-3 flex items-center gap-2
            ${leaving ? "animate-slide-out" : "animate-slide-in"}`}>

            <img className="w-16 h-16 rounded-full object-cover"
                src={userStatus.profileImageUrl ? userStatus.profileImageUrl : ProfileImage} />

            <div className="flex flex-col">
                <h2 className="text-white text-lg font-semibold">{userStatus.username}</h2>

                <span className={`flex items-center text-sm ${userStatus.isOnline ? "text-green-400" : "text-red-400"}`}>
                    <span className={`w-2 h-2 rounded-full mr-2 ${userStatus.isOnline ? "bg-green-400" : "bg-red-400"}`}></span>
                    {userStatus.isOnline ? "Online" : "Offline"}
                </span>
            </div>

        </article>
    );
}

export default FriendStatusAlert;