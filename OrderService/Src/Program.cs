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

builder.Services.AddOptions<JwtConfig>()
    .Bind(builder.Configuration.GetSection("Jwt"));
// .ValidateDataAnnotations();

builder.Services.AddSingleton<InternalJwtTokenContainer>();
builder.Services.AddScoped<JwtTokenContainerReader>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetSection("Jwt")["Authority"];
        options.RequireHttpsMetadata = false; // TODO: Only for development
        options.Audience = builder.Configuration.GetSection("Jwt")["Audience"]; // TODO: Only for development
    });

builder.Services.AddHostedService<JwtTokenRefresher>();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
