using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Application.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddOptions<PostgresConfiguration>()
    .Bind(builder.Configuration.GetSection("Database"))
    .ValidateDataAnnotations();

string? jwtSigningKey = builder.Configuration.GetSection("Jwt")["SigningKey"];
ArgumentException.ThrowIfNullOrEmpty(jwtSigningKey);

builder.Services.AddScoped<IUserAuthService, UserAuthService>();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey));
        options.TokenValidationParameters.ValidIssuer = "ecommerce-backend";
        options.TokenValidationParameters.IssuerSigningKey = key;
        options.TokenValidationParameters.ValidAlgorithms = [SecurityAlgorithms.HmacSha256];
        
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidateLifetime = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();