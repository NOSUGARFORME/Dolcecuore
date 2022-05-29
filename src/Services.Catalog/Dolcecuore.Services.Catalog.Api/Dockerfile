FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services.Catalog/Dolcecuore.Services.Catalog.Api/Dolcecuore.Services.Catalog.Api.csproj", "Services.Catalog/Dolcecuore.Services.Catalog.Api/"]
COPY ["Common/Dolcecuore.Application/Dolcecuore.Application.csproj", "Common/Dolcecuore.Application/"]
COPY ["Common/Dolcecuore.Domain/Dolcecuore.Domain.csproj", "Common/Dolcecuore.Domain/"]
COPY ["Common/Dolcecuore.CrossCuttingConcerns/Dolcecuore.CrossCuttingConcerns.csproj", "Common/Dolcecuore.CrossCuttingConcerns/"]
COPY ["Common/Dolcecuore.Infrastructure/Dolcecuore.Infrastructure.csproj", "Common/Dolcecuore.Infrastructure/"]
RUN dotnet restore "Services.Catalog/Dolcecuore.Services.Catalog.Api/Dolcecuore.Services.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Services.Catalog/Dolcecuore.Services.Catalog.Api"
RUN dotnet build "Dolcecuore.Services.Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dolcecuore.Services.Catalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Services.Catalog.Api.dll"]
