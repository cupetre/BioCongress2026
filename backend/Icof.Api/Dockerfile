# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first (separate layer so `dotnet restore` is cached unless the csproj changes)
COPY Icof.Api.csproj ./
RUN dotnet restore Icof.Api.csproj

# Copy the rest of the source and publish
COPY . .
RUN dotnet publish Icof.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Icof.Api.dll"]
