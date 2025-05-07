# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
RUN mkdir -p /app/wwwroot/images; chown $APP_UID /app/wwwroot/images
EXPOSE 8081

# This stage is used to run the service in development when running from the cli
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS development
WORKDIR /app
COPY . .
RUN mkdir -p /app/wwwroot/images; chown $APP_UID /app/wwwroot/images
CMD dotnet run --no-launch-profile

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Server.API.csproj", "."]
RUN dotnet restore "./Server.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Server.API.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:CSharpier_Bypass=true

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:CSharpier_Bypass=true

# This stage is used to apply migrations in prod environments
FROM build AS migration
WORKDIR /src
RUN dotnet tool install --version 9.0.3 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update


# This stage is used to run tests
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS test
WORKDIR /src
COPY . .
RUN dotnet tool install --global dotnet-coverage
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet test -s coverlet.runsettings Server.sln && dotnet-coverage merge /src/**/*.cobertura.xml --output merged.cobertura.xml --output-format cobertura && reportgenerator -reports:"merged.cobertura.xml" -targetdir:"coverage" -reporttypes:Html

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/wwwroot/images; chown $APP_UID /app/wwwroot/images
ENTRYPOINT ["dotnet", "Server.API.dll"]