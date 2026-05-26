# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Gem.Powerplant.sln", "."]
COPY ["Gem.Powerplant.Api/Gem.Powerplant.Api.csproj", "Gem.Powerplant.Api/"]
COPY ["Gem.Powerplant.Application/Gem.Powerplant.Application.csproj", "Gem.Powerplant.Application/"]
COPY ["Gem.Powerplant.Domain/Gem.Powerplant.Domain.csproj", "Gem.Powerplant.Domain/"]

# Restore dependencies
RUN dotnet restore "Gem.Powerplant.sln"

# Copy source code
COPY ["Gem.Powerplant.Api/", "Gem.Powerplant.Api/"]
COPY ["Gem.Powerplant.Application/", "Gem.Powerplant.Application/"]
COPY ["Gem.Powerplant.Domain/", "Gem.Powerplant.Domain/"]

# Build
RUN dotnet build "Gem.Powerplant.Api/Gem.Powerplant.Api.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "Gem.Powerplant.Api/Gem.Powerplant.Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 5001

# Set environment
ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_ENVIRONMENT=Production

# Run application
ENTRYPOINT ["dotnet", "Gem.Powerplant.Api.dll"]