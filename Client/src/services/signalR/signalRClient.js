import * as signalR from "@microsoft/signalr";

export const createConnection = () => {
    return new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7182/app", {
            accessTokenFactory: () => localStorage.getItem("accessToken")
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.None)
        .build();
};