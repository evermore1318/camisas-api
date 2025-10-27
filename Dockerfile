# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY DSW1_PROYECTO_PALACIO_CAMISAS.sln .
COPY DSW_PROYECTO_PALACIO_CAMISAS_API/DSW_PROYECTO_PALACIO_CAMISAS_API.csproj DSW_PROYECTO_PALACIO_CAMISAS_API/
COPY DSW_PROYECTO_PALACIO_CAMISAS_WebApp/DSW_PROYECTO_PALACIO_CAMISAS_WebApp.csproj DSW_PROYECTO_PALACIO_CAMISAS_WebApp/

# Restaurar dependencias
RUN dotnet restore

# Copiar todo el código
COPY . .

# Publicar la API
RUN dotnet publish DSW_PROYECTO_PALACIO_CAMISAS_API/DSW_PROYECTO_PALACIO_CAMISAS_API.csproj -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "DSW_PROYECTO_PALACIO_CAMISAS_API.dll"]