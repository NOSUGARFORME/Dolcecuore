FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services.Basket/Dolcecuore.Services.Basket.Api/Dolcecuore.Services.Basket.Api.csproj", "Services.Basket/Dolcecuore.Services.Basket.Api/"]
RUN dotnet restore "Services.Basket/Dolcecuore.Services.Basket.Api/Dolcecuore.Services.Basket.Api.csproj"
COPY . .
WORKDIR "/src/Services.Basket/Dolcecuore.Services.Basket.Api"
RUN dotnet build "Dolcecuore.Services.Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dolcecuore.Services.Basket.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Services.Basket.Api.dll"]
