# ---------- Etapa 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos primero solo los .csproj para aprovechar la cache de capas de Docker
# (si el código cambia pero las dependencias no, esta capa no se vuelve a descargar)
COPY SistemApi.Domain/SistemApi.Domain.csproj SistemApi.Domain/
COPY SistemApi.Application/SistemApi.Application.csproj SistemApi.Application/
COPY SistemApi.Infrastructure/SistemApi.Infrastructure.csproj SistemApi.Infrastructure/
COPY SistemApi.Api/SistemApi.Api.csproj SistemApi.Api/

RUN dotnet restore SistemApi.Api/SistemApi.Api.csproj

# Ahora copiamos el resto del código fuente
COPY . .

# Publicamos en modo Release
RUN dotnet publish SistemApi.Api/SistemApi.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------- Etapa 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Render inyecta la variable de entorno PORT dinámicamente.
# ASP.NET Core debe escuchar en ese puerto (no en el 5000/8080 fijo).
ENTRYPOINT ["/bin/sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-10000} dotnet SistemApi.Api.dll"]
