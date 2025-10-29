# TODO: add prompt
# Build images for minikube

cd ..

echo "Bulding docker images"

docker build -t ecommercebackend-user-service:prod ./UserService
docker build -t ecommercebackend-user-service-db-migrator:prod --target migrate ./UserService

docker build -t ecommercebackend-product-service:prod ./ProductService
docker build -t ecommercebackend-product-service-db-migrator:prod --target migrate ./ProductService

docker build -t ecommercebackend-store-service:prod ./StoreService
docker build -t ecommercebackend-store-service-db-migrator:prod --target migrate ./StoreService

docker build -t ecommercebackend-order-service:prod ./OrderService
docker build -t ecommercebackend-order-service-db-migrator:prod --target migrate ./OrderService

docker build -t ecommercebackend-payment-service:prod ./PaymentService
docker build -t ecommercebackend-payment-service-db-migrator:prod --target migrate ./PaymentService

kind load docker-image \
  ecommercebackend-user-service:prod \
  ecommercebackend-user-service-db-migrator:prod \
  ecommercebackend-product-service:prod \
  ecommercebackend-product-service-db-migrator:prod \
  ecommercebackend-store-service:prod \
  ecommercebackend-store-service-db-migrator:prod \
  ecommercebackend-order-service:prod \
  ecommercebackend-order-service-db-migrator:prod \
  ecommercebackend-payment-service:prod \
  ecommercebackend-payment-service-db-migrator:prod

kubectl create configmap user-service-config \
  --from-env-file=./UserService/.env
kubectl create configmap product-service-config \
  --from-env-file=./ProductService/.env
kubectl create configmap store-service-config \
  --from-env-file=./StoreService/.env
kubectl create configmap order-service-config \
  --from-env-file=./OrderService/.env
kubectl create configmap payment-service-config \
  --from-env-file=./PaymentService/.env

kubectl create secret generic user-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic product-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic store-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic order-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic payment-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1

read -p "Enter Stripe API Key: " stripeApiKey
read -p "Enter Stripe Webhook Secret: " stripeWebhookSecret

echo "Stripe__ApiKey=$stripeApiKey" > .stripe
echo "Stripe__WebhookSecret=$stripeWebhookSecret" >> .stripe

kubectl create secret generic stripe-secret --from-env-file=.stripe

kubectl create secret generic order-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic payment-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1

rm .stripe

kubectl create secret generic master-user-secrets \
  --from-literal=Master__Email=masteruser@gmail.com \
  --from-literal=Master__Password=Masterpassword.55

cd ./k8s/umbrella
helm dependency update
helm install umbrella .
sleep 100