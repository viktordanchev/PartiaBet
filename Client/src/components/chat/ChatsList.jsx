import React from 'react';

import ProfileImg from '../../assets/images/profile-photo.jpg';

const ChatsList = ({ activeFriend, setActiveFriend, friends }) => {
    return (
        <article className="w-1/5 h-full bg-slate-600 flex flex-col items-center py-3 gap-3">

            {friends && friends.map((friend) => {
                const isActive = activeFriend?.id === friend.id;

                return (
                    <div key={friend.id}
                        className="relative group flex flex-col items-center cursor-pointer"
                        onClick={(e) => {
                            e.stopPropagation();
                            setActiveFriend(friend);
                        }}>

                        <div className={`relative transition-all duration-300 ${isActive ? "scale-115" : "opacity-50 blur-[1px]"}`}>

                            <img src={friend.profileImageUrl || ProfileImg}
                                className={`w-10 h-10 rounded-full border-2 transition-all ${friend.isOnline ? "border-green-500" : "border-gray-500"}`}
                            />

                            <span className={`absolute bottom-0 right-0 w-3 h-3 rounded-full border-2 border-slate-600 transition-all duration-300 ${friend.isOnline ? "bg-green-500" : "bg-gray-500"}`}/>

                        </div>

                        <div className="absolute left-14 top-1/2 -translate-y-1/2 bg-black text-white text-xs px-2 py-1 rounded opacity-0 group-hover:opacity-100 transition whitespace-nowrap pointer-events-none">
                            {friend.username}
                        </div>

                    </div>
                );
            })}

        </article>
    );
};

export default ChatsList;