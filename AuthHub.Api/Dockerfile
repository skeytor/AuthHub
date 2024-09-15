# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthHub.Api/AuthHub.Api.csproj", "AuthHub.Api/"]
COPY ["AuthHub.Infrastructure/AuthHub.Infrastructure.csproj", "AuthHub.Infrastructure/"]
COPY ["AuthHub.Domain/AuthHub.Domain.csproj", "AuthHub.Domain/"]
COPY ["AuthHub.Percistence/AuthHub.Persistence.csproj", "AuthHub.Percistence/"]
RUN dotnet restore "./AuthHub.Api/AuthHub.Api.csproj"
COPY . .
WORKDIR "/src/AuthHub.Api"
RUN dotnet build "./AuthHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthHub.Api.dll"]