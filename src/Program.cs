using ECommerce.Auth;
using ECommerce.Common.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
// Add custom services
builder.Services.AddScoped<IAuthService, AuthService>();

// TODO: hide this secret key
builder.Services.AddScoped<JwtService>(_ => new JwtService("8E181A76C50E956FB62F3B620945D3BA75DF1E0A3A1B8C34CDD72FFB562379C4"));

string? connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
if (connectionString == null)
{
    throw new Exception("Connection string is empty");
}

builder.Services.AddScoped<PostgresService>(_ => new PostgresService(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();