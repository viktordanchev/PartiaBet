import { useEffect } from "react";
import { useAppHub } from "../../contexts/AppHubContext";

export const useSignalREvent = (eventName, handler) => {
    const { connection } = useAppHub();

    useEffect(() => {
        if (!connection) return;

        connection.on(eventName, handler);

        return () => {
            connection.off(eventName, handler);
        };
    }, [connection, eventName, handler]);
};