# Takes in service name(eg. OrderService)
addJwtEnvs() {
    echo "Jwt__ClientId=$jwtClientId" >> "../$1/.env"
    echo "Jwt__SecretClientId=$jwtClientSecret" >> "../$1/.env"
    echo "Jwt__Issuer=$jwtIssuer" >> "../$1/.env"
    echo "Jwt__Authority=$jwtAuthority" >> "../$1/.env"
    echo "Jwt__Audience=$jwtAudience" >> "../$1/.env"
}

productServiceUrl="http://product-service:8080"
paymentServiceUrl="http://payment-service:8080"
userServiceUrl="http://user-service:8080"
storeServiceUrl="http://store-service:8080"
orderServiceUrl="http://order-service:8080"

# Takes in service name(eg. OrderService)
addMicroserviceUrls() {
  echo "MicroserviceNetworkConfig__PaymentServiceUrl=$paymentServiceUrl" >> ../$1/.env
  echo "MicroserviceNetworkConfig__ProductServiceUrl=$productServiceUrl" >> ../$1/.env
  echo "MicroserviceNetworkConfig__UserServiceUrl=$userServiceUrl" >> ../$1/.env
  echo "MicroserviceNetworkConfig__OrderServiceUrl=$orderServiceUrl" >> ../$1/.env
  echo "MicroserviceNetworkConfig__StoreServiceUrl=$storeServiceUrl" >> ../$1/.env
}

read -p "Enter keycloak ecommerce-api client secret: " jwtClientSecret


jwtClientId=ecommerce-api
jwtClientSecret=secret
jwtAuthority="http://keycloak:8080/realms/ecommerce-api"
jwtAudience=ecommerce-api

# BFF
echo "ASPNETCORE_URLS=http://+:8080" >> ../BFF/.env
echo "Kafka__Servers=kafka:9092" >> ../BFF/.env
addJwtEnvs "BFF"
addMicroserviceUrls "BFF"

# Payment service
echo "Database__Host=payment-service-postgres" > ../PaymentService/.env 
echo "Database__Port=5432" >> ../PaymentService/.env 
echo "Database__Database=postgres" >> ../PaymentService/.env 
echo "Database__Username=postgres" >> ../PaymentService/.env 
echo "Database__Password=postgres" >> ../PaymentService/.env 
echo "Kafka__Servers=kafka" >> ../PaymentService/.env 

read -p "Enter Stripe API key(or leave empty): " stripeApiKey
read -p "Enter Stripe webhook secret(or leave empty): " stripeWebhookSecret

echo "Stripe__ApiKey=$stripeApiKey" >> ../PaymentService/.env
echo "Stripe__WebhookSecret=$stripeWebhookSecret" >> ../PaymentService/.env

addJwtEnvs "PaymentService"
addMicroserviceUrls "PaymentService"

# Product service
echo "Database__Host=product-service-postgres" > ../ProductService/.env 
echo "Database__Port=5432" >> ../ProductService/.env 
echo "Database__Database=postgres" >> ../ProductService/.env 
echo "Database__Username=postgres" >> ../ProductService/.env 
echo "Database__Password=postgres" >> ../ProductService/.env 
echo "Kafka__Servers=kafka" >> ../ProductService/.env 

addJwtEnvs "ProductService"
addMicroserviceUrls "ProductService"

# Store service
echo "Database__Host=store-service-postgres" > ../StoreService/.env 
echo "Database__Port=5432" >> ../StoreService/.env 
echo "Database__Database=postgres" >> ../StoreService/.env 
echo "Database__Username=postgres" >> ../StoreService/.env 
echo "Database__Password=postgres" >> ../StoreService/.env 
echo "Kafka__Servers=kafka" >> ../StoreService/.env 

addJwtEnvs "StoreService"
addMicroserviceUrls "StoreService"

# User service
echo "Database__Host=user-service-postgres" > ../UserService/.env 
echo "Database__Port=5432" >> ../UserService/.env 
echo "Database__Database=postgres" >> ../UserService/.env 
echo "Database__Username=postgres" >> ../UserService/.env 
echo "Database__Password=postgres" >> ../UserService/.env 
echo "Kafka__Servers=kafka" >> ../UserService/.env 

addJwtEnvs "UserService"
addMicroserviceUrls "UserService"

# Order service
echo "Database__Host=order-service-postgres" > ../OrderService/.env 
echo "Database__Port=5432" >> ../OrderService/.env 
echo "Database__Database=postgres" >> ../OrderService/.env 
echo "Database__Username=postgres" >> ../OrderService/.env 
echo "Database__Password=postgres" >> ../OrderService/.env 
echo "Kafka__Servers=kafka" >> ../OrderService/.env

addJwtEnvs "OrderService"
addMicroserviceUrls "OrderService"