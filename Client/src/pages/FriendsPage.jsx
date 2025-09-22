import React, { useEffect } from 'react';
import FriendCard from '../components/FriendsPage/FriendCard';
import SearchBar from '../components/FriendsPage/SearchBar';

function FriendsPage() {

    useEffect(() => {
        document.title = 'Friends';
    });

    return (
        <section className="flex-1 p-6 flex flex-col justify-start items-center gap-6">
            <SearchBar />
            <article className="w-full flex flex-wrap justify-center gap-3">
                <FriendCard
                    username="1234567890123456"
                    isOnline={true}
                />
                <FriendCard
                    username="1234567890"
                    isOnline={false}
                />
                <FriendCard
                    username="123456789"
                    isOnline={true}
                />
                <FriendCard
                    username="123456"
                    isOnline={true}
                />
                <FriendCard
                    username="123456"
                    isOnline={true}
                />
                <FriendCard
                    username="123456"
                    isOnline={true}
                />
                <FriendCard
                    username="123456"
                    isOnline={true}
                />
            </article>
        </section>
    );
}

export default FriendsPage;