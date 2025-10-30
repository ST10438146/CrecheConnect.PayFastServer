FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["CrecheConnect.PayFastServer/CrecheConnect.PayFastServer.csproj", "CrecheConnect.PayFastServer/"]
RUN dotnet restore "CrecheConnect.PayFastServer/CrecheConnect.PayFastServer.csproj"

COPY . .

WORKDIR "/src/CrecheConnect.PayFastServer"

RUN dotnet build "CrecheConnect.PayFastServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrecheConnect.PayFastServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

EXPOSE 80

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CrecheConnect.PayFastServer.dll"]
