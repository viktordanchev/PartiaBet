import React, { useState } from 'react';
import Loading from '../components/Loading';
import Board from '../components/MatchPage/Chess/Board';

const MatchPage = () => {
    const [isLoading, setIsLoading] = useState(false);

    return (
        <section className="flex-1 p-6 flex justify-center items-center">
            {isLoading ? <Loading size={'small'} /> :
                <div className="flex">
                    <Board />
                    <article className="h-fit py-2 px-4 bg-red-600 rounded-xl text-white border border-white space-x-1">
                        <span className="font-medium">12</span>
                        <span>Spectators</span>
                    </article>
                </div>}
        </section>
    );
};

export default MatchPage;