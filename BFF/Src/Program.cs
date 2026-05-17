using BFF.Auth;
using BFF.Cart;
using ECommerceBackend.Utils.Auth;
using ECommerceBackend.Utils.Microservices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddLogging();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddHttpClient();
builder.Services.AddOptions<KeycloakAuthOptions>()
    .Bind(builder.Configuration.GetSection(KeycloakAuthOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddHttpClient<IKeycloakTokenService, KeycloakTokenService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddOptions<MicroserviceHosts>()
    .Bind(builder.Configuration.GetSection("MicroserviceNetworkConfig"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddApplicationAuth(builder);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();
