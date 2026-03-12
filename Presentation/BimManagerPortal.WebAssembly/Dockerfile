# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Presentation/BimManagerPortal.WebAssembly/BimManagerPortal.WebAssembly.csproj", "Presentation/BimManagerPortal.WebAssembly/"]
COPY ["Shared/BimManagerPortal.Shared/BimManagerPortal.Shared.csproj", "Shared/BimManagerPortal.Shared/"]
RUN dotnet restore "Presentation/BimManagerPortal.WebAssembly/BimManagerPortal.WebAssembly.csproj"

COPY . .
WORKDIR "/src/Presentation/BimManagerPortal.WebAssembly"
RUN dotnet publish "BimManagerPortal.WebAssembly.csproj" -c Release -o /app/publish

# Финальный образ с nginx
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80