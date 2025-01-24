using System.Text;
using ECommerce.Auth;
using ECommerce.Categories;
using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using ECommerce.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
if (jwtOptions == null)
{
    throw new ArgumentException("Jwt options is null");
}

builder.Services.AddSingleton<JwtOptions>(jwtOptions);
builder.Services.AddSingleton<IJwtService, JwtService>(_ => new JwtService(jwtOptions));

string? connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
if (connectionString == null)
{
    throw new ArgumentException("Connection string is empty");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };

        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var token = context.HttpContext.Request.Cookies["user"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton<IPostgresService, PostgresService>(_ => new PostgresService(connectionString));

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<ICategoriesService, CategoriesService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();