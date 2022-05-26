FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services.Order/Dolcecuore.Services.Order.Api/Dolcecuore.Services.Order.Api.csproj", "Services.Order/Dolcecuore.Services.Order.Api/"]
COPY ["Services.Order/Dolcecuore.Services.Order.Application/Dolcecuore.Services.Order.Application.csproj", "Services.Order/Dolcecuore.Services.Order.Application/"]
COPY ["Services.Order/Dolcecuore.Services.Order.Domain/Dolcecuore.Services.Order.Domain.csproj", "Services.Order/Dolcecuore.Services.Order.Domain/"]
COPY ["Services.Order/Dolcecuore.Services.Order.Infrastructure/Dolcecuore.Services.Order.Infrastructure.csproj", "Services.Order/Dolcecuore.Services.Order.Infrastructure/"]
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
