#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/src/inventory-control-of-dep-api/inventory-control-of-dep-api.csproj", "inventory-control-of-dep-api/"]
RUN dotnet restore "inventory-control-of-dep-api/inventory-control-of-dep-api.csproj"
COPY . .
WORKDIR "src/inventory-control-of-dep-api/"
RUN dotnet build "inventory-control-of-dep-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "inventory-control-of-dep-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "inventory-control-of-dep-api.dll"]