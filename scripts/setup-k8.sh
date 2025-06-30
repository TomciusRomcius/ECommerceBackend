# TODO: check if secrets and config maps exist 
kubectl create secret generic postgres --from-literal=postgres-password=POSTGRES_PASSWORD > /dev/null 2>&1
# TODO: Not ideal as we are injecting not only master user data
kubectl create secret generic master-user-secrets --from-env-file ../.env
// TODO: migrations
kubectl create configmap postgres-init --from-file=../sql-init/init.sql
kubectl create configmap postgres-connection-config --from-env-file=../.env
cd ../k8s/umbrella
helm dependency build
helm install umbrella .
sleep 100