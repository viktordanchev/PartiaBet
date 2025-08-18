import React from 'react';
import GameCard from '../components/homePage/GameCard';
import ChessImg from '../assets/images/chess.jpg';
import BackgammonImg from '../assets/images/backgammon.png';
import BeloteImg from '../assets/images/belote.png';
import SixtySixImg from '../assets/images/sixty-six.png';

function HomePage() {
    return (
        <section className="w-full p-6 flex flex-wrap justify-center items-center gap-6">
            <GameCard
                gameImg={ChessImg}
                gameName={'Chess'}
                playersCount={12}
            />
            <GameCard
                gameImg={BackgammonImg}
                gameName={'Backgammon'}
                playersCount={57}
            />
            <GameCard
                gameImg={BeloteImg}
                gameName={'Belote'}
                playersCount={144}
            />
            <GameCard
                gameImg={SixtySixImg}
                gameName={'Sixty-Six'}
                playersCount={6}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Chess'}
                playersCount={12}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Backgammon'}
                playersCount={57}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Belote'}
                playersCount={144}
            />
            <GameCard
                gameImg={ChessImg}
                gameName={'Sixty-Six'}
                playersCount={6}
            />
        </section>
    );
}

export default HomePage;