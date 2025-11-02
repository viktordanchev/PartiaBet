import React from 'react';
import ProfilePhoto from '../../assets/images/profile-photo.jpg';
import QuestionMark from '../../assets/images/question-mark.png';

const PlayerCard = ({ data }) => {
    return (
        <div className="p-2 flex items-center gap-3 bg-gray-900 rounded border border-gray-500 shadow-xl shadow-gray-900">
            <img src={data ? (data.profileImageUrl || ProfilePhoto) : QuestionMark}
                className="rounded-lg border border-gray-500 h-12 w-12"
            />
            <div>
                <p className="font-semibold">{data ? data.username : 'Anonymous'}</p>
                <p className="text-xs">{data ? data.rating : 0}</p>
            </div>
        </div>
    );
};

export default PlayerCard;