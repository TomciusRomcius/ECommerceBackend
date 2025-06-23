FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM docker:28-dind AS test-base
RUN apk add dotnet9-sdk
ENV DOTNET_ROOT=$HOME/.dotnet
ENV PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ["ECommerce.Presentation/ECommerce.Presentation.csproj", "ECommerce.Presentation/ECommerce.Presentation.csproj"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/ECommerce.Infrastructure.csproj"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/ECommerce.Application.csproj"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/ECommerce.Domain.csproj"]

COPY ["ECommerce.Presentation.Tests/ECommerce.Presentation.Tests.csproj", "ECommerce.Presentation.Tests/ECommerce.Presentation.Tests.csproj"]
COPY ["ECommerce.Infrastructure.Tests/ECommerce.Infrastructure.Tests.csproj", "ECommerce.Infrastructure.Tests/ECommerce.Infrastructure.Tests.csproj"]
COPY ["ECommerce.Domain.Tests/ECommerce.Domain.Tests.csproj", "ECommerce.Domain.Tests/ECommerce.Domain.Tests.csproj"]

COPY ["ECommerce.Persistence/ECommerce.Persistence.csproj", "ECommerce.Persistence/ECommerce.Persistence.csproj"]

COPY ["TestUtils/TestUtils.csproj", "TestUtils/TestUtils.csproj"]

COPY ["ECommerceBackend.sln", "ECommerceBackend.sln"]

RUN dotnet restore
COPY . .
RUN dotnet build -c $BUILD_CONFIGURATION

FROM test-base AS test
WORKDIR /app
COPY --from=build /app .
COPY --from=build /root/.nuget/packages /root/.nuget/packages 
CMD [ "dotnet", "test" ]

FROM build as migrate
WORKDIR /app
ENTRYPOINT ["dotnet", "ef", "database", "update", "--no-build", "--project", "ECommerce.Persistence"]

FROM build AS publish
WORKDIR /app
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommerce.Presentation" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.Presentation.dll"]