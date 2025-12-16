jwtSigningKey="4+3XWNf4H4YeqzYKpwxo9cr3eEoGNRlTivGARi9NSgs"
productServiceUrl="http://product-service:8080"
paymentServiceUrl="http://payment-service:8080"
userServiceUrl="http://user-service:8080"

# Main API

echo "Database__Host=main-api-postgres" > ../MainApi/.env 
echo "Database__Port=5432" >> ../MainApi/.env 
echo "Database__Database=postgres" >> ../MainApi/.env
echo "Database__Username=postgres" >> ../MainApi/.env 
echo "Database__Password=postgres" >> ../MainApi/.env 
echo "Kafka__Servers=kafka" >> ../MainApi/.env 

echo "Master__Email=masteruser@gmail.com" >> ../MainApi/.env
echo "Master__Password=Masterpassword.55" >> ../MainApi/.env

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

# Product service

echo "Database__Host=product-service-postgres" > ../ProductService/.env 
echo "Database__Port=5432" >> ../ProductService/.env 
echo "Database__Database=postgres" >> ../ProductService/.env 
echo "Database__Username=postgres" >> ../ProductService/.env 
echo "Database__Password=postgres" >> ../ProductService/.env 
echo "Kafka__Servers=kafka" >> ../ProductService/.env 

# Store service

echo "Database__Host=store-service-postgres" > ../StoreService/.env 
echo "Database__Port=5432" >> ../StoreService/.env 
echo "Database__Database=postgres" >> ../StoreService/.env 
echo "Database__Username=postgres" >> ../StoreService/.env 
echo "Database__Password=postgres" >> ../StoreService/.env 
echo "Kafka__Servers=kafka" >> ../StoreService/.env 

# User service

echo "Database__Host=user-service-postgres" > ../UserService/.env 
echo "Database__Port=5432" >> ../UserService/.env 
echo "Database__Database=postgres" >> ../UserService/.env 
echo "Database__Username=postgres" >> ../UserService/.env 
echo "Database__Password=postgres" >> ../UserService/.env 
echo "Kafka__Servers=kafka" >> ../UserService/.env 
echo "Jwt__SigningKey=$jwtSigningKey" >> ../UserService/.env
echo "Jwt__Issuer=ecommerce-auth" >> ../UserService/.env
echo "Jwt__LifetimeMinutes=120" >> ../UserService/.env

# Order service

echo "Database__Host=user-service-postgres" > ../OrderService/.env 
echo "Database__Port=5432" >> ../OrderService/.env 
echo "Database__Database=postgres" >> ../OrderService/.env 
echo "Database__Username=postgres" >> ../OrderService/.env 
echo "Database__Password=postgres" >> ../OrderService/.env 
echo "Kafka__Servers=kafka" >> ../OrderService/.env
echo "MicroserviceNetworkConfig__PaymentServiceUrl=$paymentServiceUrl" >> ../OrderService/.env
echo "MicroserviceNetworkConfig__ProductServiceUrl=$productServiceUrl" >> ../OrderService/.env
echo "MicroserviceNetworkConfig__UserServiceUrl=$userServiceUrl" >> ../OrderService/.env
echo "Jwt__SigningKey=$jwtSigningKey" >> ../OrderService/.env