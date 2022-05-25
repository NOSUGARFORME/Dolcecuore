FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services.Discount/Dolcecuore.Services.Discount.Grpc/*.csproj", "Services.Discount/Dolcecuore.Services.Discount.Grpc/"]
RUN dotnet restore "./Services.Discount/Dolcecuore.Services.Discount.Grpc/Dolcecuore.Services.Discount.Grpc.csproj"
COPY . .
WORKDIR "/src/Services.Discount/Dolcecuore.Services.Discount.Grpc"
RUN dotnet build "Dolcecuore.Services.Discount.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dolcecuore.Services.Discount.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dolcecuore.Services.Discount.Grpc.dll"]
