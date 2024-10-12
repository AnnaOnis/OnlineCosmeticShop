# Этап 1: Базовый образ для ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этап 2: Сборка frontend (React)
FROM node:18 AS frontend
WORKDIR /app
COPY ["Presentation/cosmeticshop.client/package.json", "Presentation/cosmeticshop.client/package-lock.json", "./"]
RUN npm install
COPY ["./Presentation/cosmeticshop.client", "./"]
RUN npm run build && ls -la /app

# Этап 3: Сборка backend (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Presentation/CosmeticShop.WebAPI/CosmeticShop.WebAPI.csproj", "Presentation/CosmeticShop.WebAPI/"]
COPY ["Infrastructure/Persistence/CosmeticShop.DB.EF/CosmeticShop.DB.EF.csproj", "Infrastructure/Persistence/CosmeticShop.DB.EF/"]
COPY ["Domain/CosmeticShop.Domain/CosmeticShop.Domain.csproj", "Domain/CosmeticShop.Domain/"]
RUN dotnet restore "Presentation/CosmeticShop.WebAPI/CosmeticShop.WebAPI.csproj"

COPY . .
WORKDIR "/src/Presentation/CosmeticShop.WebAPI"
RUN dotnet build "CosmeticShop.WebAPI.csproj" -c Release -o /app/build

# Этап 4: Публикация
FROM build AS publish
RUN dotnet publish "CosmeticShop.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап 5: Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend /app/dist /app/wwwroot
ENTRYPOINT ["dotnet", "CosmeticShop.WebAPI.dll"]
