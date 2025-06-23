# TODO: add prompt
# Build images for minikube

cd ..

echo "Bulding docker images"

docker build -t ecommercebackend-main-api:prod ./MainApi
docker build -t ecommercebackend-main-api-db-migrator:prod --target migrate ./MainApi

docker build -t ecommercebackend-payment-service:prod ./PaymentService
docker build -t ecommercebackend-payment-service-db-migrator:prod --target migrate ./PaymentService

kind load docker-image \
  ecommercebackend-main-api:prod \
  ecommercebackend-main-api-db-migrator:prod \
  ecommercebackend-payment-service:prod \
  ecommercebackend-payment-service-db-migrator:prod

kubectl create configmap main-api-config \
  --from-env-file=./MainApi/.env
kubectl create configmap payment-service-config \
  --from-env-file=./PaymentService/.env

kubectl create secret generic main-api-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic payment-service-db-secret --from-literal=Database__Password=POSTGRES_PASSWORD > /dev/null 2>&1

read -p "Enter Stripe API Key: " stripeApiKey
read -p "Enter Stripe Webhook Secret: " stripeWebhookSecret

echo "Stripe__ApiKey=$stripeApiKey" > .stripe
echo "Stripe__WebhookSecret=$stripeWebhookSecret" >> .stripe

kubectl create secret generic stripe-secret --from-env-file=.stripe
rm .stripe

kubectl create secret generic master-user-secrets \
  --from-literal=Master__Email=masteruser@gmail.com \
  --from-literal=Master__Password=Masterpassword.55

cd ./k8s/umbrella
helm dependency update
helm install umbrella .
sleep 100