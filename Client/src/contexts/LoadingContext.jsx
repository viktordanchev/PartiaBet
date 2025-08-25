import { createContext, useContext, useState } from 'react';
import Loading from '../components/Loading';

const LoadingContext = createContext();

export const LoadingProvider = ({ children }) => {
    const [isLoading, setIsLoading] = useState(false);

    return (
        <LoadingContext.Provider value={{ setIsLoading }}>
            {isLoading && (
                <div className="fixed z-50 flex inset-0 items-center justify-center bg-black/40">
                    <Loading size="big" />
                </div>
            )}
            {children}
        </LoadingContext.Provider>
    );
};

export const useLoading = () => useContext(LoadingContext);
