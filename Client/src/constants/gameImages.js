import ChessImg from '../assets/images/chess/chess.png';
import BeloteImg from '../assets/images/belote/belote.png';
import SixtySixImg from '../assets/images/sixty-six/sixty-six.png';
import BackgammonImg from '../assets/images/backgammon/backgammon.png';

const gameImages = {
    Chess: ChessImg,
    Belote: BeloteImg,
    'Sixty-Six': SixtySixImg,
    Backgammon: BackgammonImg
};

const getGameImage = (gameName) => {
    return gameImages[gameName];
};

export default getGameImage;