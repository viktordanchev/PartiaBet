import React, { useState, useEffect, useRef } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMessage } from '@fortawesome/free-solid-svg-icons';
import useApiRequest from '../../hooks/useApiRequest';
import { useClickOutside } from '../../hooks/useClickOutside';
import { useAuth } from '../../contexts/AuthContext';

import ChatWindow from './ChatWindow';
import FriendsList from './FriendsList';
import Loading from '../Loading';

const Chat = () => {
    const { isAuthenticated } = useAuth();
    const containerRef = useRef(null);
    const apiRequest = useApiRequest();

    const [isChatOpen, setIsChatOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [activeFriend, setActiveFriend] = useState(null);
    const [friends, setFriends] = useState([]);

    useClickOutside(containerRef, () => setIsChatOpen(false));

    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);
            const friends = await apiRequest('friends', 'getFriendships', 'GET', true);
            setIsLoading(false);

            setFriends(friends);
        };

        if (isAuthenticated) {
            receiveData();
        }
    }, [isAuthenticated]);

    if (!isAuthenticated) return;

    return (
        <section className={`fixed right-6 bottom-6 border border-gray-500 text-white flex overflow-hidden rounded-xl transition-all duration-500 ease-in-out ${isChatOpen ? "w-90 h-110 bg-white" : "w-15 h-15 cursor-pointer items-center justify-center bg-blue-500"}`}
            onClick={() => !isChatOpen && setIsChatOpen(true)}
            ref={containerRef}>

            {(isChatOpen && isLoading) && <Loading size={'small'} />}

            {!isChatOpen ? (
                <FontAwesomeIcon icon={faMessage} className="text-2xl text-gray-300" />
            ) : (
                <div className="h-full w-full flex">
                    <FriendsList
                        activeFriend={activeFriend}
                        setActiveFriend={setActiveFriend}
                        friends={friends}
                    />
                    <ChatWindow
                        setIsChatOpen={setIsChatOpen}
                        activeFriend={activeFriend}
                    />
                </div>
            )}

        </section>
    );
};

export default Chat;