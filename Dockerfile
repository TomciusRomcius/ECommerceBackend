FROM mcr.microsoft.com/dotnet/sdk:9.0 as base
WORKDIR /app
COPY . .
EXPOSE 5042
CMD [ "dotnet", "watch", "--project", "ECommerceBackend" ]