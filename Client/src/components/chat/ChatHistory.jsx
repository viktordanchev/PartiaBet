import React, { useState, useEffect, useRef } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowDown } from '@fortawesome/free-solid-svg-icons';
import { jwtDecode } from 'jwt-decode';
import { useSignalREvent } from "../../hooks/signalR/useSignalREvent";

const ChatHistory = ({ messages }) => {
    const containerRef = useRef(null);
    const typingTimeoutRef = useRef(null);

    const [showScrollButton, setShowScrollButton] = useState(false);
    const [isTyping, setIsTyping] = useState(false);

    const decodedToken = jwtDecode(localStorage.getItem('accessToken'));
    const userId = decodedToken['Id'];

    const scrollToBottom = () => {
        const el = containerRef.current;
        if (!el) return;

        el.scrollTo({
            top: el.scrollHeight,
            behavior: "smooth"
        });
    };

    const isAtBottom = () => {
        const el = containerRef.current;
        if (!el) return false;

        return el.scrollHeight - el.scrollTop - el.clientHeight < 150;
    };

    const handleScroll = () => {
        const el = containerRef.current;
        if (!el) return;

        const atBottom = el.scrollHeight - el.scrollTop - el.clientHeight < 150;
        setShowScrollButton(!atBottom);
    };

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
        scrollToBottom();
    }, []);

    useEffect(() => {
        const el = containerRef.current;
        if (!el) return;

        el.addEventListener("scroll", handleScroll);

        handleScroll();

        return () => el.removeEventListener("scroll", handleScroll);
    }, []);

    useEffect(() => {
        if (isAtBottom()) {
            scrollToBottom();
        }
    }, [isTyping]);

    return (
        <div className="relative h-9/11 bg-white">

            <div className="h-full p-3 text-sm flex flex-col gap-2 overflow-y-auto scrollbar-hide"
                ref={containerRef}>

                {messages.map((msg) => {
                    const isMine = msg.senderId === userId;

                    return (
                        <div key={msg.id} className="relative">

                            <div className={`group flex ${isMine ? "justify-end" : "justify-start"}`}>

                                <div className="relative">

                                    <div className={`px-3 py-2 rounded-lg max-w-xs break-words ${isMine
                                            ? "bg-blue-500 text-white rounded-br-none"
                                            : "bg-gray-200 text-gray-800 rounded-bl-none"
                                        }`}>
                                        {msg.message}
                                    </div>

                                    {/* TOOLTIP */}
                                    <div className={`absolute z-20 top-full mt-1 ${isMine ? "right-0" : "left-0"} bg-slate-800/80 text-white text-xs px-2 py-1 rounded opacity-0 group-hover:opacity-100 transition whitespace-nowrap pointer-events-none`}>
                                        {new Date(msg.dateAndTime).toLocaleString()}
                                    </div>

                                </div>

                            </div>

                        </div>
                    );
                })}

                {isTyping && (
                    <div className="flex justify-start">
                        <div className="bg-gray-200 text-gray-800 rounded-bl-none px-3 py-3 rounded-lg flex gap-1 items-center">
                            <span className="w-2 h-2 bg-gray-500 rounded-full animate-typing"></span>
                            <span className="w-2 h-2 bg-gray-500 rounded-full animate-typing" style={{ animationDelay: '0.3s' }}></span>
                            <span className="w-2 h-2 bg-gray-500 rounded-full animate-typing" style={{ animationDelay: '0.6s' }}></span>
                        </div>
                    </div>
                )}

            </div>

            {showScrollButton && (
                <button onClick={scrollToBottom}
                    className="absolute bottom-3 left-1/2 -translate-x-1/2 w-8 h-8 bg-slate-600 rounded-full flex items-center justify-center shadow-lg transition-all duration-300 hover:scale-110 cursor-pointer"
                >
                    <FontAwesomeIcon icon={faArrowDown} className="text-white text-base" />
                </button>
            )}

        </div>
    );
};

export default ChatHistory;