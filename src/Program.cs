using ECommerce.Common.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
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