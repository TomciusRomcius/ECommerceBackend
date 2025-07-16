using ECommerce.Application.src.UseCases.Manufacturer.Commands;
using ECommerce.Application.src.UseCases.Manufacturer.Handlers;
using ECommerce.Application.src.UseCases.Manufacturer.Queries;
using ECommerce.Application.Tests.Utils;
using ECommerce.Domain.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Tests.Integration
{
    public class ManufacturerHandlersTest : DbContextWithDependencyInjection
    {
        public ManufacturerHandlersTest() : base() { }

        protected override void PreServiceProviderCreation(IServiceCollection services)
        {
            services.AddScoped<CreateManufacturerHandler>();
            services.AddScoped<GetAllManufacturersHandler>();
        }

        [Fact]
        public async Task CreateManufacturer_ShouldCreateAManufacturer()
        {
            CreateManufacturerHandler handler = ServiceProvider.GetRequiredService<CreateManufacturerHandler>();

            string manufacturerName = "New manufacturer";

            // Act
            await handler.Handle(new CreateManufacturerCommand(manufacturerName), CancellationToken.None);

            // Assert
            List<ManufacturerEntity> retrievedManufacturers = await DbContext.Manufacturers.ToListAsync();

            Assert.Single(retrievedManufacturers);
            Assert.Equal(manufacturerName, retrievedManufacturers[0].Name);
            Assert.True(retrievedManufacturers[0].ManufacturerId >= 0);
        }

        [Fact]
        public async Task GetAllManufacturers_ShouldGetAllManufacturers()
        {
            // Arrange
            GetAllManufacturersHandler handler = ServiceProvider.GetRequiredService<GetAllManufacturersHandler>();

            string[] manufacturers = ["Manufacturer1", "Manufacturer2", "Manufacturer3"];
            await DbContext.Manufacturers.AddRangeAsync(manufacturers.Select(name => new ManufacturerEntity(name)));
            await DbContext.SaveChangesAsync();

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
    }
}
