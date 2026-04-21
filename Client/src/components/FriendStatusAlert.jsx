import React, { useState, useRef } from 'react';
import ProfileImage from '../assets/images/profile-photo.jpg';
import { useSignalREvent } from "../hooks/signalR/useSignalREvent";

function FriendStatusAlert() {
    const [visible, setVisible] = useState(false);
    const [leaving, setLeaving] = useState(false);
    const [friendStatus, setFriendStatus] = useState(null);

    const hideTimerRef = useRef(null);
    const leaveTimerRef = useRef(null);

    useSignalREvent("FriendStatusChanged", (userData, isOnline) => {
        const data = {
            username: userData.username,
            profileImageUrl: userData.profileImageUrl,
            isOnline: isOnline
        };

        setFriendStatus(data);
        setVisible(true);
        setLeaving(false);

        if (hideTimerRef.current) clearTimeout(hideTimerRef.current);
        if (leaveTimerRef.current) clearTimeout(leaveTimerRef.current);

        hideTimerRef.current = setTimeout(() => {
            setLeaving(true);

            leaveTimerRef.current = setTimeout(() => {
                setVisible(false);
                setFriendStatus(null);
            }, 400);

        }, 3000);
    });

    if (!friendStatus || !visible) return null;

    return (
        <article className={`h-20 w-60 bg-gray-900 rounded-xl border border-gray-500 shadow-xl shadow-gray-900 p-3 flex items-center gap-2
            ${leaving ? "animate-slide-out" : "animate-slide-in"}`}>

            <img
                className="w-12 h-12 rounded-full object-cover"
                src={friendStatus.profileImageUrl || ProfileImage}
                alt="profile"
            />

            <div className="flex flex-col">
                <h2 className="text-white text-md font-semibold">
                    {friendStatus.username}
                </h2>

                <span className={`flex items-center text-sm ${friendStatus.isOnline ? "text-green-400" : "text-red-400"}`}>
                    <span className={`w-2 h-2 rounded-full mr-2 ${friendStatus.isOnline ? "bg-green-400" : "bg-red-400"}`}></span>
                    {friendStatus.isOnline ? "Online" : "Offline"}
                </span>
            </div>

        </article>
    );
}

export default FriendStatusAlert;