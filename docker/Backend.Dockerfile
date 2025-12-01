# Etapa 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copiamos solución y proyectos necesarios
COPY ["dotnet/dotnet.sln", "./"]
COPY ["dotnet/ZSports.Api/ZSports.Api.csproj", "ZSports.Api/"]
COPY ["dotnet/ZSports.Persistence/ZSports.Persistence.csproj", "ZSports.Persistence/"]
COPY ["dotnet/ZSports.Domain/ZSports.Domain.csproj", "ZSports.Domain/"]
COPY ["dotnet/ZSports.Contracts/ZSports.Contracts.csproj", "ZSports.Contracts/"]

RUN dotnet restore "ZSports.Api/ZSports.Api.csproj"

# Copiamos el resto del código
COPY ["dotnet/", "./"]

RUN dotnet publish "ZSports.Api/ZSports.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ZSports.Api.dll"]