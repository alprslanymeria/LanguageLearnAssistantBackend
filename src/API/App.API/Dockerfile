# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/App.API/App.API.csproj", "src/API/App.API/"]
COPY ["src/core/App.Application/App.Application.csproj", "src/core/App.Application/"]
COPY ["src/core/App.Domain/App.Domain.csproj", "src/core/App.Domain/"]
COPY ["src/infastructure/App.Caching/App.Caching.csproj", "src/infastructure/App.Caching/"]
COPY ["src/shared/App.Shared/App.Shared.csproj", "src/shared/App.Shared/"]
COPY ["src/infastructure/App.Integration/App.Integration.csproj", "src/infastructure/App.Integration/"]
COPY ["src/infastructure/App.Observability/App.Observability.csproj", "src/infastructure/App.Observability/"]
COPY ["src/infastructure/App.Persistence/App.Persistence.csproj", "src/infastructure/App.Persistence/"]
COPY ["src/infastructure/App.Security/App.Security.csproj", "src/infastructure/App.Security/"]
COPY ["src/infastructure/App.Storage/App.Storage.csproj", "src/infastructure/App.Storage/"]
RUN dotnet restore "./src/API/App.API/App.API.csproj"
COPY . .
WORKDIR "/src/src/API/App.API"
RUN dotnet build "./App.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# MIGRATION STAGE
FROM build AS migration
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /src
RUN dotnet build "src/API/App.API/App.API.csproj" -c Release
ENTRYPOINT ["dotnet", "ef", "database", "update", \
            "--project", "src/infastructure/App.Persistence/App.Persistence.csproj", \
            "--startup-project", "src/API/App.API/App.API.csproj", \
            "--no-build", \
            "--configuration", "Release"]

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./App.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# CURL INSTALLATION FOR HEALTH CHECKS
USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID


COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.API.dll"]
