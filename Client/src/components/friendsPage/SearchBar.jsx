import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';

const SearchBar = () => {
    return (
        <article className="w-100 flex items-center rounded-3xl border-4 border-gray-900 shadow-xl shadow-gray-900">
            <input
                placeholder="Search friend..."
                className="text-lg border-2 border-r-0 border-gray-300 rounded-s-3xl bg-gray-300 h-9 p-4 focus:outline-none focus:border-maincolor sm:w-full"
            >
            </input>
            <button className="bg-maincolor h-9 w-9 rounded-e-3xl hover:scale-110">
                <FontAwesomeIcon icon={faMagnifyingGlass} className="text-gray-900 text-xl hover:cursor-pointer" />
            </button>
        </article>
    );
};

export default SearchBar;