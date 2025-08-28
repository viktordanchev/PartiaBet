import React, { useState } from 'react';
import { useParams } from "react-router-dom";
import Loading from '../components/Loading';
import ChessMatch from '../components/MatchPage/Chess/ChessMatch';
import Spectators from '../components/MatchPage/Spectators';

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
        <section className="flex-1 p-6 flex justify-center gap-3">
            {isLoading ? <Loading size={'small'} /> :
                <>
                    <div className="flex flex-col items-end gap-3">
                        <Spectators peopleCount={125} />
                        <p className="text-2xl font-semibold text-gray-300">Total 40$</p>
                    </div>
                    {renderGame()}
                </>}
        </section>
    );
};

export default MatchPage;