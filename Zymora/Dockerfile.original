FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zymora/Zymora.csproj", "Zymora/"]
COPY ["Zymora_BE.Repositories/Zymora_BE.Repositories.csproj", "Zymora_BE.Repositories/"]
COPY ["Zymora_BE.Contract.Repositories/Zymora_BE.Contract.Repositories.csproj", "Zymora_BE.Contract.Repositories/"]
COPY ["Zymora_BE.Contract.Services/Zymora_BE.Contract.Services.csproj", "Zymora_BE.Contract.Services/"]
COPY ["Zymora_BE.Services/Zymora_BE.Services.csproj", "Zymora_BE.Services/"]
COPY ["Zymora_BE.Middleware/Zymora_BE.Middleware.csproj", "Zymora_BE.Middleware/"]
COPY ["Zymora_BE.Core/Zymora_BE.Core.csproj", "Zymora_BE.Core/"]

RUN dotnet restore "Zymora/Zymora.csproj"

COPY . .

WORKDIR /src/Zymora
RUN dotnet build "Zymora.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zymora.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zymora.dll"]
