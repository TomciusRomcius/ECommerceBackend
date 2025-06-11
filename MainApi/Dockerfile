FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /app
COPY ["ECommerce.Presentation/ECommerce.Presentation.csproj", "ECommerce.Presentation/ECommerce.Presentation.csproj"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/ECommerce.Infrastructure.csproj"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/ECommerce.Application.csproj"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/ECommerce.Domain.csproj"]

COPY ["ECommerce.Presentation.Tests/ECommerce.Presentation.Tests.csproj", "ECommerce.Presentation.Tests/ECommerce.Presentation.Tests.csproj"]
COPY ["ECommerce.Infrastructure.Tests/ECommerce.Infrastructure.Tests.csproj", "ECommerce.Infrastructure.Tests/ECommerce.Infrastructure.Tests.csproj"]
COPY ["ECommerce.Domain.Tests/ECommerce.Domain.Tests.csproj", "ECommerce.Domain.Tests/ECommerce.Domain.Tests.csproj"]
COPY ["TestUtils/TestUtils.csproj", "TestUtils/TestUtils.csproj"]

COPY ["ECommerceBackend.sln", "ECommerceBackend.sln"]

RUN dotnet restore
COPY . .
RUN dotnet build -c $BUILD_CONFIGURATION -o ../app-build/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ECommerce.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.Presentation.dll"]