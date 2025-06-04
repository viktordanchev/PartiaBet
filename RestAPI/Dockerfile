FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PartiaBet.Server/PartiaBet.Server.csproj", "PartiaBet.Server/"]
COPY ["partiabet.client/partiabet.client.esproj", "partiabet.client/"]
RUN dotnet restore "./PartiaBet.Server/PartiaBet.Server.csproj"
COPY . .
WORKDIR "/src/PartiaBet.Server"
RUN dotnet build "./PartiaBet.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PartiaBet.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PartiaBet.Server.dll"]