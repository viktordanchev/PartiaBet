import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMessage } from '@fortawesome/free-solid-svg-icons';

import OpenedChat from './OpenedChat';
import ChatsList from './ChatsList';

const Chat = () => {
    const [isChatOpen, setIsChatOpen] = useState(false);
    const [activeFriend, setActiveFriend] = useState(null);

    return (
        <section className={`fixed right-6 bottom-6 border border-gray-500 text-white bg-blue-500 flex overflow-hidden rounded-xl transition-all duration-500 ease-in-out ${isChatOpen ? "w-90 h-110" : "w-15 h-15 cursor-pointer items-center justify-center"}`}
            onClick={() => !isChatOpen && setIsChatOpen(true)}>

            {!isChatOpen ? (
                <FontAwesomeIcon icon={faMessage} className="text-2xl text-gray-300" />
            ) : (
                <div className="h-full w-full flex">
                    <ChatsList
                        activeFriend={activeFriend}
                        setActiveFriend={setActiveFriend}
                    />
                    <OpenedChat
                        setIsChatOpen={setIsChatOpen}
                        activeFriend={activeFriend}
                    />
                </div>
            )}

        </section>
    );
};

export default Chat;