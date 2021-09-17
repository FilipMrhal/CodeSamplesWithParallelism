FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MainThreadTesting/MainThreadTesting.csproj", "MainThreadTesting/"]
RUN dotnet restore "MainThreadTesting/MainThreadTesting.csproj"
COPY . .
WORKDIR "/src/MainThreadTesting"
RUN dotnet build "MainThreadTesting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainThreadTesting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainThreadTesting.dll"]
