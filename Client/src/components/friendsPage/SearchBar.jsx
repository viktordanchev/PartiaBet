import React, { useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import useApiRequest from '../../hooks/useApiRequest';

const SearchBar = ({ setUsers, setIsLoading }) => {
    const apiRequest = useApiRequest();
    const [fieldText, setFieldText] = useState('');
    const [username, setUsername] = useState('');

    useEffect(() => {
        const receiveData = async () => {
            setIsLoading(true);
            const users = await apiRequest('friends', 'searchPlayers', 'POST', true, false, username);
            setIsLoading(false);
            
            setUsers(users);
        };

        if (username) {
            receiveData();
        }
    }, [username]);

    const handleSearch = () => {
        setUsername(fieldText);
    };

    const handleEnterDown = (e) => {
        if (e.key === 'Enter') {
            handleSearch();
        }
    };

    return (
        <article className="w-100 flex items-center rounded-3xl shadow-xl shadow-gray-900">

            <input className="text-lg border-2 border-r-0 border-gray-300 rounded-s-3xl bg-gray-300 h-9 p-4 focus:outline-none focus:border-maincolor sm:w-full"
                placeholder="Search player..."
                value={fieldText}
                onChange={(e) => setFieldText(e.target.value)}
                onKeyDown={handleEnterDown}
            />

            <button className="bg-maincolor h-9 w-9 rounded-e-3xl hover:scale-110 hover:cursor-pointer"
                onClick={handleSearch}
            >
                <FontAwesomeIcon icon={faMagnifyingGlass} className="text-gray-900 text-xl" />
            </button>

        </article>
    );
};

export default SearchBar;