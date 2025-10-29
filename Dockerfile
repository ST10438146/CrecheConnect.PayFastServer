# ============================================
# 1️⃣ Build Stage
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["CrecheConnect.PayFastServer/CrecheConnect.PayFastServer.csproj", "CrecheConnect.PayFastServer/"]
RUN dotnet restore "CrecheConnect.PayFastServer/CrecheConnect.PayFastServer.csproj"

# Copy all source code
COPY . .

# Set working directory to project folder
WORKDIR "/src/CrecheConnect.PayFastServer"

# Build the project in Release mode
RUN dotnet build "CrecheConnect.PayFastServer.csproj" -c Release -o /app/build

# ============================================
# 2️⃣ Publish Stage
# ============================================
FROM build AS publish
RUN dotnet publish "CrecheConnect.PayFastServer.csproj" -c Release -o /app/publish

# ============================================
# 3️⃣ Runtime Stage
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Expose port 80 for Render
EXPOSE 80

# Copy published output
COPY --from=publish /app/publish .

# Entry point
ENTRYPOINT ["dotnet", "CrecheConnect.PayFastServer.dll"]
