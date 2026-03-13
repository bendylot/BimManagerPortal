FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Очистка NuGet кэша
RUN dotnet nuget locals all --clear

# Копируем только csproj файлы
COPY ["Presentation/BimManagerPortal.WebApi/BimManagerPortal.WebApi.csproj", "Presentation/BimManagerPortal.WebApi/"]
COPY ["Core/BimManagerPortal.Application/BimManagerPortal.Application.csproj", "Core/BimManagerPortal.Application/"]
COPY ["Shared/BimManagerPortal.Shared/BimManagerPortal.Shared.csproj", "Shared/BimManagerPortal.Shared/"]
COPY ["Core/BimManagerPortal.Domain/BimManagerPortal.Domain.csproj", "Core/BimManagerPortal.Domain/"]
COPY ["Infrastructure/BimManagerPortal.Persistance/BimManagerPortal.Persistance.csproj", "Infrastructure/BimManagerPortal.Persistance/"]

# Создаем NuGet.config для настройки таймаута (альтернативный способ)
RUN echo '<?xml version="1.0" encoding="utf-8"?> \
<configuration> \
  <config> \
    <add key="http_proxy.timeout" value="600" /> \
  </config> \
</configuration>' > NuGet.config

# Восстанавливаем без --timeout, но с retry логикой
RUN dotnet restore "Presentation/BimManagerPortal.WebApi/BimManagerPortal.WebApi.csproj" --disable-parallel || \
    (echo "First restore failed, retrying with cleared cache..." && \
     dotnet nuget locals http-cache --clear && \
     dotnet restore "Presentation/BimManagerPortal.WebApi/BimManagerPortal.WebApi.csproj" --disable-parallel)

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