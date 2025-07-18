# TODO: add prompt
# Build images for minikube
eval $(minikube docker-env)
cd ..

docker build -t main-api:latest ./MainApi
docker build -t main-api-db-migrator:latest --target migrate ./MainApi

docker build -t payment-service:latest ./PaymentService
docker build -t payment-service-db-migrator:latest --target migrate ./PaymentService

kubectl create configmap main-api-config \
  --from-env-file=../MainApi/.env
kubectl create configmap payment-service-config \
  --from-env-file=../PaymentService/.env

kubectl create secret generic main-api-db-secret --from-literal=postgres-password=POSTGRES_PASSWORD > /dev/null 2>&1
kubectl create secret generic payment-service-db-secret --from-literal=postgres-password=POSTGRES_PASSWORD > /dev/null 2>&1

read -p "Enter Stripe API Key: " stripeApiKey
read -p "Enter Stripe Webhook Secret: " stripeWebhookSecret

echo "Stripe__ApiKey=$stripeApiKey" > .stripe
echo "Stripe__WebhookSecret=$stripeWebhookSecret" >> .stripe
kubectl create secret generic stripe-secret --from-env-file=.stripe
rm .stripe

kubectl create secret generic master-user-secrets \
  --from-literal=Master__Email=masteruser@gmail.com \
  --from-literal=Master__Password=Masterpassword.55

cd ../k8s/umbrella
helm dependency update
helm install umbrella .
sleep 100