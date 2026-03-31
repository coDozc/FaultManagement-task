# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["FaultManagement.sln", "./"]
COPY ["FaultManagement.Api/FaultManagement.Api.csproj", "FaultManagement.Api/"]
COPY ["FaultManagement.Domain/FaultManagement.Domain.csproj", "FaultManagement.Domain/"]
COPY ["FaultManagement.Application/FaultManagement.Application.csproj", "FaultManagement.Application/"]
COPY ["FaultManagement.Infrastructure/FaultManagement.Infrastructure.csproj", "FaultManagement.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "FaultManagement.sln"

# Copy application code
COPY . .

# Build
RUN dotnet build "FaultManagement.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FaultManagement.Api/FaultManagement.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5155
ENTRYPOINT ["dotnet", "FaultManagement.Api.dll"]
