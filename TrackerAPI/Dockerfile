# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TrackerAPI.csproj", "./"]
RUN dotnet restore "TrackerAPI.csproj"
COPY . .
RUN dotnet publish "TrackerAPI.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrackerAPI.dll"]