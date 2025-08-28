import React, { useState } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/MatchPage/Chess/ChessMatch';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye } from '@fortawesome/free-regular-svg-icons';

const MatchPage = () => {
    const { game } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    
    const renderGame = () => {
        switch (game) {
            case "chess":
                return <ChessMatch />;
            case "dota":
                return <DotaMatch />;
            case "csgo":
                return <CsgoMatch />;
            default:
                return <p>Game not supported</p>;
        }
    };

    return (
        <section className="flex-1 p-6 flex justify-center items-center">
            {isLoading ? <Loading size={'small'} /> :
                <div className="flex gap-3">
                    <article className="h-fit py-2 px-3 bg-red-600 rounded-xl text-white font-medium border border-white space-x-1">
                        <span className="">12</span>
                        <FontAwesomeIcon icon={faEye} />
                    </article>
                    {renderGame()}
                </div>}
        </section>
    );
};

export default MatchPage;