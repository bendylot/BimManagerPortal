FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Очистка NuGet кэша и настройка источников
RUN dotnet nuget locals all --clear

# Копируем solution file и все csproj файлы
COPY ["BimManagerPortal.sln", "./"]
COPY ["Presentation/BimManagerPortal.WebApi/BimManagerPortal.WebApi.csproj", "Presentation/BimManagerPortal.WebApi/"]
COPY ["Core/BimManagerPortal.Application/BimManagerPortal.Application.csproj", "Core/BimManagerPortal.Application/"]
COPY ["Shared/BimManagerPortal.Shared/BimManagerPortal.Shared.csproj", "Shared/BimManagerPortal.Shared/"]
COPY ["Core/BimManagerPortal.Domain/BimManagerPortal.Domain.csproj", "Core/BimManagerPortal.Domain/"]
COPY ["Infrastructure/BimManagerPortal.Persistance/BimManagerPortal.Persistance.csproj", "Infrastructure/BimManagerPortal.Persistance/"]

# Восстанавливаем с таймаутом и retry логикой
RUN dotnet restore "BimManagerPortal.sln" \
    --timeout 600 \
    --disable-parallel \
    || (echo "First restore failed, retrying..." && \
        dotnet nuget locals http-cache --clear && \
        dotnet restore "BimManagerPortal.sln" --timeout 600)

# Копируем остальной код
COPY . .
WORKDIR "/src/Presentation/BimManagerPortal.WebApi"
RUN dotnet build "./BimManagerPortal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BimManagerPortal.WebApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BimManagerPortal.WebApi.dll"]