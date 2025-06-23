using ECommerce.Domain.src.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence
{
    public class DatabaseContext : IdentityDbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ManufacturerEntity> Manufacturers { get; set; }
        public DbSet<ShippingAddressEntity> ShippingAddresses { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }
    }
}
