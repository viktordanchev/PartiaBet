import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark, faPaperPlane } from '@fortawesome/free-solid-svg-icons';

const OpenedChat = ({ setIsChatOpen, activeFriend }) => {

    if (!activeFriend) {
        return (
            <div className="flex-1 flex items-center justify-center text-gray-600 bg-white">
                Select a friend to start chatting
            </div>
        );
    }

    return (
        <div className="w-4/5 h-full flex flex-col justify-between text-white">

            <div className="h-1/9 bg-slate-600 px-3 flex justify-between items-center">

                <p className="text-lg">
                    {activeFriend.name}
                </p>

                <FontAwesomeIcon
                    icon={faXmark}
                    className="text-2xl cursor-pointer"
                    onClick={(e) => {
                        e.stopPropagation();
                        setIsChatOpen(false);
                    }}/>

            </div>

            <div className="flex-1 p-3 text-sm text-gray-600 bg-white">
                Chat with {activeFriend.name}
            </div>

            <div className="h-1/9 bg-slate-600 px-3 flex items-center gap-3">

                <input className="w-full focus:outline-none"
                    placeholder={`Message...`}
                    type="text"/>

                <button className="cursor-pointer flex justify-center items-center">
                    <FontAwesomeIcon className="text-xl" icon={faPaperPlane} />
                </button>

            </div>

        </div>
    );
};

export default OpenedChat;