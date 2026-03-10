FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/BimManagerPortal.WebApi/BimManagerPortal.WebApi.csproj", "Presentation/BimManagerPortal.WebApi/"]
RUN dotnet restore "Presentation/BimManagerPortal.WebApi/BimManagerPortal.WebApi.csproj"
COPY . .
WORKDIR "/src/Presentation/BimManagerPortal.WebApi"
RUN dotnet build "./BimManagerPortal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BimManagerPortal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BimManagerPortal.WebApi.dll"]
