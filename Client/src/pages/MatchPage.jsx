import React, { useState } from 'react';
import Loading from '../components/Loading';
import Board from '../components/MatchPage/Chess/Board';

const MatchPage = () => {
    const [isLoading, setIsLoading] = useState(false);
   
    return (
        <section className="flex-1 p-6 flex justify-center items-center">
            {isLoading ? <Loading size={'small'} /> : <Board />}
        </section>
    );
};

export default MatchPage;