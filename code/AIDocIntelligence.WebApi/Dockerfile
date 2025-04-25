# Base stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 5005

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Copy project file and restore dependencies
COPY ["AIDocIntelligence.WebApi.csproj", "./"]
RUN dotnet restore "./AIDocIntelligence.WebApi.csproj"

# Copy the rest of the files and build the application
COPY . .
WORKDIR "/src"
RUN dotnet build "./AIDocIntelligence.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./AIDocIntelligence.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for debugging
FROM base AS final

# Switch to root for package installation
USER root

WORKDIR /app

# Install Visual Studio Debugger
RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

# Switch back to non-root user
USER app

# Copy published app
COPY --from=publish /app/publish .

# Set environment to Development for debugging
ENV ASPNETCORE_ENVIRONMENT=Development

# Debug entry point for remote debugging
CMD ["dotnet", "AIDocIntelligence.WebApi.dll", "--server.urls", "http://0.0.0.0:8080", "--launch-profile", "Development"]
