#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Adventure.Server/Adventure.Server.csproj", "Adventure.Server/"]
COPY ["Adventure.Core/Adventure.Core.csproj", "Adventure.Core/"]
RUN dotnet restore "Adventure.Server/Adventure.Server.csproj"
COPY . .
WORKDIR "/src/Adventure.Server"
RUN dotnet build "Adventure.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adventure.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adventure.Server.dll"]