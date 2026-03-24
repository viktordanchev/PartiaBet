import React from 'react';
import { useNavigate } from "react-router-dom";
import ProfileImage from '../../assets/images/profile-photo.jpg';

const FriendCard = ({ data }) => {
    const navigate = useNavigate();

    const navigateToUserPage = () => {
        navigate(`/player/${data.id}`);
    };

    return (
        <article className="relative h-25 w-70 bg-gray-900 rounded-xl border border-gray-500 shadow-xl shadow-gray-900 p-3 flex items-center gap-2 transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:border-white hover:scale-105"
            onClick={navigateToUserPage}>

            <img className="w-16 h-16 rounded-full object-cover"
                src={data.profileImageUrl ? data.profileImageUrl : ProfileImage}/>

            <div className="flex flex-col">
                <h2 className="text-white text-lg font-semibold">{data.username}</h2>

                <span className={`flex items-center text-sm ${data.isOnline ? "text-green-400" : "text-red-400"}`}>
                    <span className={`w-2 h-2 rounded-full mr-2 ${data.isOnline ? "bg-green-400" : "bg-red-400"}`}></span>
                    {data.isOnline ? "Online" : "Offline"}
                </span> 
            </div>

            {data.isFriendRequestPending && (
                <span className="absolute bottom-3 right-3 text-yellow-200 text-sm font-semibold">
                    Pending...
                </span>
            )}
        </article>
    );
};

export default FriendCard;