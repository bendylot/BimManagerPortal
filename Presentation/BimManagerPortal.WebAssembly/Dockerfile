FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/BimManagerPortal.WebAssembly/BimManagerPortal.WebAssembly.csproj", "Presentation/BimManagerPortal.WebAssembly/"]
COPY ["Shared/BimManagerPortal.Shared/BimManagerPortal.Shared.csproj", "Shared/BimManagerPortal.Shared/"]
RUN dotnet restore "Presentation/BimManagerPortal.WebAssembly/BimManagerPortal.WebAssembly.csproj"
COPY . .
WORKDIR "/src/Presentation/BimManagerPortal.WebAssembly"
RUN dotnet build "./BimManagerPortal.WebAssembly.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BimManagerPortal.WebAssembly.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BimManagerPortal.WebAssembly.dll"]
