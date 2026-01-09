import React from 'react';
import QuestionMark from '../../../assets/images/question-mark.png';
import ProfilePhoto from '../../../assets/images/profile-photo.jpg';

const PlayerCard = ({ data }) => {
    return (
        <div className="flex flex-col items-center text-gray-300 text-center">
            <img src={data ? (data.profileImageUrl || ProfilePhoto) : QuestionMark}
                className="rounded-lg border border-white 0 h-20 w-20"
            />
            <p className="font-semibold">{data ? data.username : 'Anonymous'}</p>
            <p>Rating: {data ? data.rating : 0}</p>
        </div>
    );
};

export default PlayerCard;