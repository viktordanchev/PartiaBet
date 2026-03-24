import React, { useEffect, useState } from 'react';
import FriendCard from '../components/friendsPage/FriendCard';
import SearchBar from '../components/friendsPage/SearchBar';
import Loading from '../components/Loading';
import useApiRequest from '../hooks/useApiRequest';

function FriendsPage() {
    const apiRequest = useApiRequest();
    const [users, setUsers] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        document.title = 'Friends';
    }, []);

    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);
            const friends = await apiRequest('friends', 'getFriendships', 'GET', true);
            setIsLoading(false);

            setUsers(friends);
        };

        receiveData();
    }, []);

    return (
        <section className="flex-1 p-6 flex flex-col justify-start items-center gap-6">

            <SearchBar setUsers={setUsers} setIsLoading={setIsLoading} />

            <>{isLoading ? <Loading size={'big'} /> :
                <article className="w-full flex flex-wrap justify-center gap-3">
                    {users.map((user) => (
                        <FriendCard
                            key={user.id}
                            data={user}
                        />
                    ))}
                </article>}</>

        </section>
    );
}

export default FriendsPage;