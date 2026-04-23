import React, { useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark, faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import useApiRequest from '../../hooks/useApiRequest';
import { useAppHub } from '../../contexts/AppHubContext';
import { useSignalREvent } from "../../hooks/signalR/useSignalREvent";

import ChatHistory from './ChatHistory';
import Loading from '../Loading';

const ChatWindow = ({ setIsChatOpen, activeFriend }) => {
    const apiRequest = useApiRequest();
    const { connection } = useAppHub();

    const [isLoading, setIsLoading] = useState(true);
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');

    const sendMessage = async () => {
        await connection.invoke('SendMessage', {
            receiverId: activeFriend.id,
            message: input
        });

        setInput('');
    };

    const senderTyping = async () => {
        await connection.invoke('SenderTyping', activeFriend.id);
    };

    const handleEnterPress = (event) => {
        if (event.key === 'Enter' && input) {
            sendMessage();
        }
    }

    useSignalREvent("ReceiveMessage", (messageData) => {
        setMessages(prev => [...prev, messageData]);
    });

    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);
            const messages = await apiRequest('chat', 'getHistory', 'POST', true, false, activeFriend.id);
            setIsLoading(false);

            setMessages(messages);
        };

        if (activeFriend) {
            receiveData();
        }
    }, [activeFriend]);

    if (!activeFriend) {
        return (
            <div className="flex-1 flex items-center justify-center text-gray-600 bg-white">
                Select a friend to start chatting
            </div>
        );
    }

    return (
        <div className="w-4/5 h-full flex flex-col justify-between text-white">

            <div className="h-1/11 bg-slate-600 px-3 flex justify-between items-center">

                <p className="text-lg">
                    {activeFriend.username}
                </p>

                <FontAwesomeIcon
                    icon={faXmark}
                    className="text-2xl cursor-pointer"
                    onClick={(e) => {
                        e.stopPropagation();
                        setIsChatOpen(false);
                    }}/>

            </div>

            {isLoading ? <div className="h-9/11 bg-slate-600/40"><Loading size={'small'} /></div> : <ChatHistory messages={messages} />}

            <div className="h-1/11 bg-slate-600 px-3 flex items-center gap-3">

                <input className="w-full focus:outline-none"
                    placeholder={`Message...`}
                    type="text"
                    value={input}
                    onKeyDown={handleEnterPress}
                    onChange={(e) => {
                        setInput(e.target.value);
                        senderTyping();
                    }} />

                <button className="cursor-pointer flex justify-center items-center"
                    onClick={sendMessage}>
                    <FontAwesomeIcon className="text-lg" icon={faPaperPlane} />
                </button>

            </div>

        </div>
    );
};

export default ChatWindow;