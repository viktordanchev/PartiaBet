import React from 'react';
import QuestionMark from '../../../assets/images/question-mark.png';
import ProfilePhoto from '../../../assets/images/profile-photo.jpg';

const PlayerCard = ({ data }) => {
    return (
        <div className="flex flex-col items-center">
            <img
                src={player.profileImgUrl || ProfilePhoto}
                className="rounded-lg border border-gray-500 h-12 w-12"
            />
            <p className="font-semibold truncate w-24">{player.username}</p>
            <p>Rating: {player.rating}</p>
        </div>
    );
};

export default PlayerCard;