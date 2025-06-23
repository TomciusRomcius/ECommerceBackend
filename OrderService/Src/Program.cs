using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrderService.InitializeOrder;
using OrderService.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddHttpClient();
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(
        Assembly.GetExecutingAssembly()
    )
);

builder.Services.AddScoped<IOrderService, OrderService.InitializeOrder.OrderService>();
builder.Services.AddScoped<IPaymentSessionService, PaymentSessionService>();
builder.Services.AddScoped<IOrderPriceCalculator, OrderPriceCalculator>();
builder.Services.AddOptions<MicroserviceNetworkConfig>()
    .Bind(builder.Configuration.GetSection("MicroserviceNetworkConfig"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

string? jwtSigningKey = builder.Configuration.GetSection("Jwt")["SigningKey"];
ArgumentException.ThrowIfNullOrEmpty(jwtSigningKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey));
        options.TokenValidationParameters.ValidIssuer = "ecommerce-backend";
        options.TokenValidationParameters.IssuerSigningKey = key;
        options.TokenValidationParameters.ValidAlgorithms = [SecurityAlgorithms.HmacSha256];
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidateIssuer = false;
        options.TokenValidationParameters.ValidateIssuerSigningKey = false;
        options.TokenValidationParameters.ValidateLifetime = false;
    });

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();
app.Run();
