# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo
COPY . .

# Publicar el proyecto WebApp (tu frontend MVC)
RUN dotnet publish DSW_PROYECTO_PALACIO_CAMISAS_WebApp/DSW_PROYECTO_PALACIO_CAMISAS_WebApp.csproj -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "DSW_PROYECTO_PALACIO_CAMISAS_WebApp.dll"]