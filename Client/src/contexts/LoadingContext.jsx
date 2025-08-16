import { createContext, useContext, useState } from 'react';
import Dice from '../assets/images/dice.png';

const LoadingContext = createContext();

export const LoadingProvider = ({ children }) => {
    const [isLoading, setIsLoading] = useState(false);

    return (
        <LoadingContext.Provider value={{ setIsLoading }}>
            {isLoading && (
                <div className="fixed z-50 flex inset-0 items-center justify-center bg-black/40">
                    <img src={Dice} className="mt-3 rotate w-30" />
                </div>
            )}
            {children}
        </LoadingContext.Provider>
    );
};

export const useLoading = () => useContext(LoadingContext);
