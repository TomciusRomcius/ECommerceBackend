using System.Reflection;
using ECommerceBackend.Utils.Auth;
using OrderService.InitializeOrder;
using OrderService.Payment;
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
builder.Services.AddScoped<IUserCartService, UserCartService>();

builder.Services.AddOptions<MicroserviceNetworkConfig>()
    .Bind(builder.Configuration.GetSection("MicroserviceNetworkConfig"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddApplicationAuth(builder);
builder.Services.AddBackgroundJwtRefresher();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseApplicationAuth();
app.MapControllers();
app.Run();
