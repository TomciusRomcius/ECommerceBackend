using ECommerce.Application.src.UseCases.Manufacturer.Commands;
using ECommerce.Application.src.UseCases.Manufacturer.Handlers;
using ECommerce.Application.src.UseCases.Manufacturer.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TestUtils;

namespace ECommerce.Application.Tests.Integration
{
    public class ManufacturerHandlersTest : IAsyncDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PostgreSqlContainer _container;

        public ManufacturerHandlersTest()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddScoped<CreateManufacturerHandler>();
            services.AddScoped<GetAllManufacturersHandler>();
            _container = TestDbUtils.CreateDbCtxAndDb(services);
            _serviceProvider = services.BuildServiceProvider();
            TestDbUtils.Migrate(_serviceProvider);
        }

        [Fact]
        public async Task CreateManufacturer_ShouldCreateAManufacturer()
        {
            // Arrange
            using IServiceScope scope = _serviceProvider.CreateScope();
            CreateManufacturerHandler handler = _serviceProvider.GetRequiredService<CreateManufacturerHandler>();
            DatabaseContext dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();

            string manufacturerName = "New manufacturer";

            // Act
            await handler.Handle(new CreateManufacturerCommand(manufacturerName), CancellationToken.None);

            // Assert
            List<ManufacturerEntity> retrievedManufacturers = await dbContext.Manufacturers.ToListAsync();

            Assert.Single(retrievedManufacturers);
            Assert.Equal(manufacturerName, retrievedManufacturers[0].Name);
            Assert.True(retrievedManufacturers[0].ManufacturerId >= 0);
        }

        [Fact]
        public async Task GetAllManufacturers_ShouldGetAllManufacturers()
        {
            // Arrange
            using IServiceScope scope = _serviceProvider.CreateScope();
            GetAllManufacturersHandler handler = _serviceProvider.GetRequiredService<GetAllManufacturersHandler>();
            DatabaseContext dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();

            string[] manufacturers = ["Manufacturer1", "Manufacturer2", "Manufacturer3"];
            await dbContext.Manufacturers.AddRangeAsync(manufacturers.Select(name => new ManufacturerEntity(name)));
            await dbContext.SaveChangesAsync();

            // Act
            List<ManufacturerEntity> retrievedManufacturers = await handler.Handle(
                new GetAllManufacturersQuery(),
                CancellationToken.None
            );

            // Assert
            Assert.Equal(manufacturers.Length, retrievedManufacturers.Count);
            foreach (var manufacturer in retrievedManufacturers)
            {
                Assert.Contains(manufacturer.Name, manufacturers);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}
