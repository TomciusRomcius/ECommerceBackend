FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /app
COPY ["PaymentService.Presentation/PaymentService.Presentation.csproj", "PaymentService.Presentation/PaymentService.Presentation.csproj"]
COPY ["PaymentService.Infrastructure/PaymentService.Infrastructure.csproj", "PaymentService.Infrastructure/PaymentService.Infrastructure.csproj"]
COPY ["PaymentService.Application/PaymentService.Application.csproj", "PaymentService.Application/PaymentService.Application.csproj"]
COPY ["PaymentService.Domain/PaymentService.Domain.csproj", "PaymentService.Domain/PaymentService.Domain.csproj"]

RUN dotnet restore "./PaymentService.Presentation/PaymentService.Presentation.csproj"
COPY . .
RUN dotnet build -c $BUILD_CONFIGURATION -o ../app-build/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.Presentation.dll"]