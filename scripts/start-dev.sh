cd ..
docker compose -f docker-compose.yml -f docker-compose-debug-tools.yml -f MainApi/docker-compose.yml -f PaymentService/docker-compose.yml up
sleep 5