### Etapa 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copiar archivos de solución y proyectos
COPY ["dotnet.sln", "./"]
COPY ["ZSports.Api/ZSports.Api.csproj", "ZSports.Api/"]
COPY ["ZSports.Persistence/ZSports.Persistence.csproj", "ZSports.Persistence/"]
COPY ["ZSports.Domain/ZSports.Domain.csproj", "ZSports.Domain/"]
COPY ["ZSports.Contracts/ZSports.Contracts.csproj", "ZSports.Contracts/"]

# Restaurar dependencias
RUN dotnet restore "ZSports.Api/ZSports.Api.csproj"

# Copiar el resto del código fuente
COPY . ./

# Publicar la aplicación en modo Release
RUN dotnet publish "ZSports.Api/ZSports.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

### Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
EXPOSE 8080

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /app/publish .

# Variable de entorno opcional para el puerto de Kestrel
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ZSports.Api.dll"]