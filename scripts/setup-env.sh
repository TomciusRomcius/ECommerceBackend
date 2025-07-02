cd ../
echo "MainApi__Database__Host=postgres" > .env 
echo "MainApi__Database__Port=5432" >> .env 
echo "MainApi__Database__Username=postgres" >> .env 
echo "MainApi__Database__Database=postgres" >> .env

echo "MainApi__Master__Email=masteruser@gmail.com" >> .env
echo "MainApi__Master__Password=Masterpassword.55" >> .env

echo "PaymentService__Database__Host=postgres" >> .env 
echo "PaymentService__Database__Port=5432" >> .env 
echo "PaymentService__Database__Username=postgres" >> .env 
echo "PaymentService__Database__Database=postgres" >> .env 

read -p "Enter Stripe API key(or leave empty): " stripeApiKey
read -p "Enter Stripe webhook secret(or leave empty): " stripeWebhookSecret

echo "PaymentService__Stripe__ApiKey=$stripeApiKey" >> .env
echo "PaymentService__Stripe__WebhookSecret=$stripeWebhookSecret" >> .env