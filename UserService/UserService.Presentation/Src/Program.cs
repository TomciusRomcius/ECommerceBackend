using ECommerceBackend.Utils.Auth;
using ECommerceBackend.Utils.Database;
using Microsoft.AspNetCore.Identity;
using UserService.Application;
using UserService.Application.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddOptions<PostgresConfiguration>()
    .Bind(builder.Configuration.GetSection("Database"))
    .ValidateDataAnnotations();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddApplicationAuth(builder);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseRouting();

app.UseApplicationAuth();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();