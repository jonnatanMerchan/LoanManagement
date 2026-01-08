# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["LoanManagement.API/LoanManagement.API.csproj", "LoanManagement.API/"]
COPY ["LoanManagement.Application/LoanManagement.Application.csproj", "LoanManagement.Application/"]
COPY ["LoanManagement.Domain/LoanManagement.Domain.csproj", "LoanManagement.Domain/"]
COPY ["LoanManagement.Infrastructure/LoanManagement.Infrastructure.csproj", "LoanManagement.Infrastructure/"]

RUN dotnet restore "LoanManagement.API/LoanManagement.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/LoanManagement.API"
RUN dotnet build "LoanManagement.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "LoanManagement.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoanManagement.API.dll"]