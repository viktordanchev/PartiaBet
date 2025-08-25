import React from 'react';
import Dice from '../assets/images/dice.png';

function Loading({ size }) {
    let width = '';

    switch (size) {
        case 'small':
            width = 'w-20';
            break;
        case 'big':
            width = 'w-30';
            break;
    }

    return (
        <img src={Dice} className={`animate-[spin_2s_linear_infinite] ${width}`} />
    );
}

export default Loading;