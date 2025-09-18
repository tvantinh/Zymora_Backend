# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Zymora/Zymora.csproj", "Zymora/"]
COPY ["Zymora_BE.Middleware/Zymora_BE.Middleware.csproj", "Zymora_BE.Middleware/"]
COPY ["Zymora_BE.Services/Zymora_BE.Services.csproj", "Zymora_BE.Services/"]
COPY ["Zymora_BE.Contract.Services/Zymora_BE.Contract.Services.csproj", "Zymora_BE.Contract.Services/"]
COPY ["Zymora_BE.Repositories/Zymora_BE.Repositories.csproj", "Zymora_BE.Repositories/"]
COPY ["Zymora_BE.Contract.Repositories/Zymora_BE.Contract.Repositories.csproj", "Zymora_BE.Contract.Repositories/"]
COPY ["Zymora_BE.Core/Zymora_BE.Core.csproj", "Zymora_BE.Core/"]
RUN dotnet restore "./Zymora/Zymora.csproj"
COPY . .
WORKDIR "/src/Zymora"
RUN dotnet build "./Zymora.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Zymora.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zymora.dll"]