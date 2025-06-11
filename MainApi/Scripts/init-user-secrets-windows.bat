echo "Initializing user secrets"
set /p "email=Enter master user email(it can be a non-existent email)"
set /p "password=Enter master user password"
set /p "stripe_key=Enter stripe api key(you can leave it empty, but you will not be able to proceed with orders)"

cd ..

dotnet user-secrets set MASTER_USER_EMAIL %email% --project ECommerce.Presentation
dotnet user-secrets set MASTER_USER_PASSWORD %password% --project ECommerce.Presentation
dotnet user-secrets set STRIPE_API_KEY %stripe_key% --project ECommerce.Presentation

pause