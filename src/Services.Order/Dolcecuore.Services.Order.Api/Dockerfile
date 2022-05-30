FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services.Order/Dolcecuore.Services.Order.Api/Dolcecuore.Services.Order.Api.csproj", "Services.Order/Dolcecuore.Services.Order.Api/"]
COPY ["Common/Dolcecuore.Application/Dolcecuore.Application.csproj", "Common/Dolcecuore.Application/"]
COPY ["Common/Dolcecuore.CrossCuttingConcerns/Dolcecuore.CrossCuttingConcerns.csproj", "Common/Dolcecuore.CrossCuttingConcerns/"]
COPY ["Common/Dolcecuore.Domain/Dolcecuore.Domain.csproj", "Common/Dolcecuore.Domain/"]
COPY ["Common/Dolcecuore.Infrastructure/Dolcecuore.Infrastructure.csproj", "Common/Dolcecuore.Infrastructure/"]
COPY ["Services.Order/Dolcecuore.Services.Order/Dolcecuore.Services.Order.csproj", "Services.Order/Dolcecuore.Services.Order/"]
RUN dotnet restore "Services.Order/Dolcecuore.Services.Order.Api/Dolcecuore.Services.Order.Api.csproj"
COPY . .
WORKDIR "/src/Services.Order/Dolcecuore.Services.Order.Api"
RUN dotnet build "Dolcecuore.Services.Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dolcecuore.Services.Order.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Services.Order.Api.dll"]
