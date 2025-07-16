# Used for development purposes in minikube
eval $(minikube docker-env)

cd ..
docker build -t main-api:latest ./MainApi
docker build -t main-api-db-migrator:latest --target migrate ./MainApi

docker build -t payment-service:latest ./PaymentService
docker build -t payment-service-db-migrator:latest --target migrate ./PaymentService

sleep 10