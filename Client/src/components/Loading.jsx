import React from 'react';
import Dice from '../assets/images/dice.png';

function Loading({ size }) {
    let width = '';

    switch (size) {
        case 'small':
            width = 'w-15';
            break;
        case 'big':
            width = 'w-30';
            break;
    }

    return (
        <div className="fixed z-50 flex inset-0 items-center justify-center bg-black/40">
            <img src={Dice} className={`animate-[spin_2s_linear_infinite] ${width}`} />
        </div>
    );
}

export default Loading;