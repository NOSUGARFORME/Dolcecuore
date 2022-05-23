FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./Services.Discount/Dolcecuore.Services.Discount.Api/*.csproj ./Services.Discount/Dolcecuore.Services.Discount.Api/

RUN dotnet restore ./Services.Discount/Dolcecuore.Services.Discount.Api/Dolcecuore.Services.Discount.Api.csproj
COPY . ./
WORKDIR /src/Services.Discount/Dolcecuore.Services.Discount.Api
RUN dotnet build Dolcecuore.Services.Discount.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Dolcecuore.Services.Discount.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Services.Discount.Api.dll"]