import React from 'react';
import QuestionMark from '../../../assets/images/question-mark.png';
import ProfilePhoto from '../../../assets/images/profile-photo.jpg';

const PlayerCard = ({ data }) => {
    return (
        <div className="flex flex-col gap-3 items-center text-gray-300 text-center">
            <img src={data ? (data.profileImageUrl || ProfilePhoto) : QuestionMark}
                className="rounded-lg border border-white h-20 w-20"
            />
            <div>
                <p className="font-semibold leading-none">{data ? data.username : 'Anonymous'}</p>
                <p className="font-light">Rating: {data ? data.rating : 0}</p>
            </div>
        </div>
    );
};

export default PlayerCard;