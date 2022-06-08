FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dolcecuore.Gateways.WebAPI/Dolcecuore.Gateways.WebAPI.csproj", "Gateways.WebAPI/Dolcecuore.Gateways.WebAPI/"]
RUN dotnet restore "Dolcecuore.Gateways.WebAPI/Dolcecuore.Gateways.WebAPI.csproj"
COPY . .
WORKDIR "/src/Gateways.WebAPI/Dolcecuore.Gateways.WebAPI"
RUN dotnet build "Dolcecuore.Gateways.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dolcecuore.Gateways.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Gateways.WebAPI.dll"]
