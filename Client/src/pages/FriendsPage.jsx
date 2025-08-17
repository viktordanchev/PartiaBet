import React from 'react';

function FriendsPage() {
    return (
        <section className="flex-grow p-6 flex">
            <article className="h-25 w-fit bg-gray-900 rounded-xl border border-gray-500 shadow-xl shadow-gray-900 p-3 flex items-center gap-2 transform transition-transform duration-300 hover:scale-105 hover:cursor-pointer hover:border-white">
                <img src="https://via.placeholder.com/80"
                    className="w-16 h-16 rounded-full object-cover border-2 border-gray-500"/>
                <div className="flex flex-col">
                    <h2 className="text-white text-lg font-semibold">Desmoetaerusssss</h2>
                    <span className="flex items-center text-sm text-green-400">
                        <span className="w-2 h-2 rounded-full bg-green-400 mr-2"></span>
                        Online
                    </span>
                </div>
            </article>
        </section>
    );
}

export default FriendsPage;