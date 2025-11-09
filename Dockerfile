FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["WeatherDashboardAPI.csproj", "./"]
RUN dotnet restore "WeatherDashboardAPI.csproj"

# Copy everything and build
COPY . .
RUN dotnet build "WeatherDashboardAPI.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "WeatherDashboardAPI.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherDashboardAPI.dll"]