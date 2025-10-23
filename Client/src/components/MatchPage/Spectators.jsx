import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye } from '@fortawesome/free-regular-svg-icons';

const Spectators = ({ peopleCount }) => {
    return (
        <article className="h-fit w-fit py-2 px-3 bg-red-600 rounded-xl text-white font-medium space-x-2">
            <span>{peopleCount}</span>
            <FontAwesomeIcon icon={faEye} />
        </article>
    );
};

export default Spectators;