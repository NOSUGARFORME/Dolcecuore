FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ./Services.Catalog/Dolcecuore.Services.Catalog.Api/*.csproj ./Services.Catalog/Dolcecuore.Services.Catalog.Api/

RUN dotnet restore ./Services.Catalog/Dolcecuore.Services.Catalog.Api/Dolcecuore.Services.Catalog.Api.csproj
COPY . ./
WORKDIR /src/Services.Catalog/Dolcecuore.Services.Catalog.Api
RUN dotnet build Dolcecuore.Services.Catalog.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Dolcecuore.Services.Catalog.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Services.Catalog.Api.dll"]