import React, { useState } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/MatchPage/Chess/ChessMatch';

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
                <div className="flex">
                    {renderGame()}
                    <article className="h-fit py-2 px-4 bg-red-600 rounded-xl text-white border border-white space-x-1">
                        <span className="font-medium">12</span>
                        <span>Spectators</span>
                    </article>
                </div>}
        </section>
    );
};

export default MatchPage;