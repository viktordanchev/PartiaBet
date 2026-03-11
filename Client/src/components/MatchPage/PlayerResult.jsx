import React from "react";

const PlayerResult = ({ data }) => {
    const ratingDiff = data.newRating - data.currentRating;

    return (
        <div className="flex items-center gap-4 bg-gray-800 p-3 rounded-xl">

            <img className="w-12 h-12 rounded-full object-cover border-2 border-maincolor"
                src={data.profileImageUrl}
                alt={data.username}/>

            <div className="flex-1 text-left">

                <p className="font-semibold">
                    {data.username}
                </p>

                <p className="text-sm text-gray-400">
                    New Rating {data.newRating}

                    <span className={`ml-4 font-semibold ${ratingDiff > 0 ? "text-green-400" : "text-red-400"}`}>
                        {ratingDiff > 0 && "+"}{ratingDiff}       
                    </span>

                </p>

            </div>

        </div>
    );
};

export default PlayerResult;