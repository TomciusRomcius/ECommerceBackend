cd ../
echo "Database__Host=postgres" > ./MainApi/.env 
echo "Database__Port=5432" >> ./MainApi/.env 
echo "Database__Database=postgres" >> ./MainApi/.env
echo "Database__Username=postgres" >> ./MainApi/.env 
echo "Database__Password=postgres" >> ./MainApi/.env 

echo "Master__Email=masteruser@gmail.com" >> ./MainApi/.env
echo "Master__Password=Masterpassword.55" >> ./MainApi/.env

echo "Database__Host=postgres" >> ./PaymentService/.env 
echo "Database__Port=5432" >> ./PaymentService/.env 
echo "Database__Database=postgres" >> ./PaymentService/.env 
echo "Database__Username=postgres" >> ./PaymentService/.env 
echo "Database__Password=postgres" >> ./PaymentService/.env 

read -p "Enter Stripe API key(or leave empty): " stripeApiKey
read -p "Enter Stripe webhook secret(or leave empty): " stripeWebhookSecret

echo "Stripe__ApiKey=$stripeApiKey" >> ./PaymentService/.env
echo "Stripe__WebhookSecret=$stripeWebhookSecret" >> ./PaymentService/.env