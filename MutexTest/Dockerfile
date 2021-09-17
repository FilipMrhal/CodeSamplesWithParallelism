FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MutexTest/MutexTest.csproj", "MutexTest/"]
RUN dotnet restore "MutexTest/MutexTest.csproj"
COPY . .
WORKDIR "/src/MutexTest"
RUN dotnet build "MutexTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MutexTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MutexTest.dll"]
