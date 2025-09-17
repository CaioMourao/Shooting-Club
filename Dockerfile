FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ShootingClub.sln", "."]
COPY ["src/Backend/ShootingClub.API/ShootingClub.API.csproj", "src/Backend/ShootingClub.API/"]
COPY ["src/Backend/ShootingClub.Application/ShootingClub.Application.csproj", "src/Backend/ShootingClub.Application/"]
COPY ["src/Backend/ShootingClub.Domain/ShootingClub.Domain.csproj", "src/Backend/ShootingClub.Domain/"]
COPY ["src/Backend/ShootingClub.Infrastructure/ShootingClub.Infrastructure.csproj", "src/Backend/ShootingClub.Infrastructure/"]
COPY ["src/Shared/ShootingClub.Communication/ShootingClub.Communication.csproj", "src/Shared/ShootingClub.Communication/"]
COPY ["src/Shared/ShootingClub.Exceptions/ShootingClub.Exceptions.csproj", "src/Shared/ShootingClub.Exceptions/"]

RUN dotnet restore "ShootingClub.sln"

COPY . .

WORKDIR "/src/src/Backend/ShootingClub.API"
RUN dotnet publish "ShootingClub.API.csproj" -c Release -o /app/publish --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ShootingClub.API.dll"]