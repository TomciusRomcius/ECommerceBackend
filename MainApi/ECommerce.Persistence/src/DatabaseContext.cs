using ECommerce.Domain.src.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.src
{
    public class DatabaseContext : IdentityDbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CartProductEntity> CartProducts { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ManufacturerEntity> Manufacturers { get; set; }
        public DbSet<StoreLocationEntity> StoreLocations { get; set; }
        public DbSet<ProductStoreLocationEntity> ProductStoreLocations { get; set; }
        public DbSet<ShippingAddressEntity> ShippingAddresses { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CartProductEntity>()
                .HasKey(cp => new { cp.UserId, cp.ProductId, cp.StoreLocationId });

            builder.Entity<ProductStoreLocationEntity>()
                .HasKey(psl => new { psl.ProductId, psl.StoreLocationId });

            base.OnModelCreating(builder);
        }
    }
}
