FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM node:lts-buster-slim AS node_base
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
COPY --from=node_base . .

WORKDIR /src
RUN mkdir training
COPY ["training.csproj", "training/"]
RUN dotnet restore "training/training.csproj"
WORKDIR "/src/training/"
COPY "." .

ENV NODE_ENV=production
RUN dotnet build "training.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "training.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "training.dll"]

