import React, { useState, useEffect } from 'react';

const ChatHistory = () => {
    const currentUserId = 1;
    const [isTyping, setIsTyping] = useState(false);
    const messages = [
        {
            id: 1,
            senderId: 1,
            text: "Здрасти, как си? Аз в момента работя по един React проект и доста се забавлявам с него.",
        },
        {
            id: 2,
            senderId: 2,
            text: "Супер съм, благодаря! Аз също гледам да уча нови неща, главно backend и малко Node.js.",
        },
        {
            id: 3,
            senderId: 1,
            text: "Яко!"
        },
        {
            id: 4,
            senderId: 2,
            text: "Звучи интересно. ",
        },
        {
            id: 5,
            senderId: 2,
            text: "Може после да добавиш и real-time с WebSockets, ще стане доста професионално.",
        },
        {
            id: 6,
            senderId: 1,
            text: "Да, точно това мислех!",
        },
    ];

    const handleScroll = () => {
        const el = containerRef.current;
        if (!el) return;

        const threshold = 50; // px tolerance
        const isBottom =
            el.scrollHeight - el.scrollTop - el.clientHeight < threshold;

        isAtBottomRef.current = isBottom;
    };

    // 👇 auto-scroll при нови съобщения
    useEffect(() => {
        const el = containerRef.current;
        if (!el) return;

        if (isAtBottomRef.current) {
            el.scrollTop = el.scrollHeight;
        }
    }, [messages.length]);

    useEffect(() => {
        const startTyping = setTimeout(() => {
            setIsTyping(true);

            // след 3 сек спира и идва ново съобщение
            setTimeout(() => {
                setIsTyping(false);
            }, 30400);
        }, 2000);

        return () => clearTimeout(startTyping);
    }, []);

    return (
        <div className="flex-1 p-3 text-sm bg-white flex flex-col gap-2 overflow-y-auto scrollbar-hide">
            {messages.map((msg) => {
                const isMine = msg.senderId === currentUserId;

                return (
                    <div
                        key={msg.id}
                        className={`flex ${isMine ? "justify-end" : "justify-start"}`}
                    >
                        <div
                            className={`px-3 py-2 rounded-lg max-w-xs break-words ${isMine
                                    ? "bg-blue-500 text-white rounded-br-none"
                                    : "bg-gray-200 text-gray-800 rounded-bl-none"
                                }`}
                        >
                            {msg.text}
                        </div>
                    </div>
                );
            })}

            {isTyping &&
                <div className="flex justify-start">
                    <div className="bg-gray-200 text-gray-800 rounded-bl-none px-3 py-3 rounded-lg flex gap-1 items-center">
                        <span className="w-2 h-2 bg-gray-500 rounded-full animate-typing"></span>
                        <span className="w-2 h-2 bg-gray-500 rounded-full animate-typing" style={{ animationDelay: '0.3s' }}></span>
                        <span className="w-2 h-2 bg-gray-500 rounded-full animate-typing" style={{ animationDelay: '0.6s' }}></span>
                    </div>
                </div>}
        </div>
    );
};

export default ChatHistory;