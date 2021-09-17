FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BackgroundThreadTesting/BackgroundThreadTesting.csproj", "BackgroundThreadTesting/"]
RUN dotnet restore "BackgroundThreadTesting/BackgroundThreadTesting.csproj"
COPY . .
WORKDIR "/src/BackgroundThreadTesting"
RUN dotnet build "BackgroundThreadTesting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackgroundThreadTesting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackgroundThreadTesting.dll"]
