FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Trackify.Api/Trackify.Api.csproj", "Trackify.Api/"]
COPY ["Trackify.Core/Trackify.Core.csproj", "Trackify.Core/"]
COPY ["Trackify.Data/Trackify.Data.csproj", "Trackify.Data/"]
RUN dotnet restore "./Trackify.Api/Trackify.Api.csproj"
COPY . .
WORKDIR "/src/Trackify.Api"
RUN dotnet build "./Trackify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Trackify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trackify.Api.dll"]
