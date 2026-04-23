import React, { useState, useEffect, useRef } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark, faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { jwtDecode } from 'jwt-decode';
import useApiRequest from '../../hooks/useApiRequest';
import { useAppHub } from '../../contexts/AppHubContext';
import { useSignalREvent } from "../../hooks/signalR/useSignalREvent";

import ChatHistory from './ChatHistory';
import Loading from '../Loading';

const OpenedChat = ({ setIsChatOpen, activeFriend }) => {
    const typingTimeoutRef = useRef(null);
    const apiRequest = useApiRequest();
    const { connection } = useAppHub();
    const [isLoading, setIsLoading] = useState(true);
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');
    const [isTyping, setIsTyping] = useState(false);

    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));

    const sendMessage = async () => {
        const newMessage = {
            message: input,
            senderId: decodedToken['Id'],
            receiverId: activeFriend.id,
        };

        setMessages(prev => [...prev, newMessage]);

        await connection.invoke('SendMessage', {
            receiverId: newMessage.receiverId,
            message: newMessage.message
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

    useSignalREvent("ReceiveMessage", (senderId, message) => {
        const newMessage = {
            message,
            senderId,
            receiverId: decodedToken['Id']
        };

        setMessages(prev => [...prev, newMessage]);
    });

    useSignalREvent("SenderTyping", () => {
        setIsTyping(true);

        if (typingTimeoutRef.current) {
            clearTimeout(typingTimeoutRef.current);
        }

        typingTimeoutRef.current = setTimeout(() => {
            setIsTyping(false);
        }, 3000);
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

            {isLoading ? <Loading size={'small'} /> : <ChatHistory messages={messages} isTyping={isTyping} />}

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

export default OpenedChat;