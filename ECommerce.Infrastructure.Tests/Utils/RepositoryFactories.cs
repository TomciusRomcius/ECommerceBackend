using ECommerce.Domain.Validators.Category;
using ECommerce.Domain.Validators.Product;
using ECommerce.Domain.Validators.User;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ECommerce.Infrastructure.Tests.Utils
{
    public static class RepositoryFactories
    {
        public static UserRepository CreateUserRepository(IPostgresService postgresService) =>
            new UserRepository(
                postgresService,
                new Mock<ILogger<UserRepository>>().Object,
                new UserEntityValidator(),
                new UpdateUserModelValidator()
            );

        public static ProductRepository CreateProductRepository(IPostgresService postgresService) =>
            new ProductRepository(
                postgresService,
                new ProductEntityValidator(),
                new UpdateProductModelValidator()
            );

        public static CategoryRepository CreateCategoryRepository(IPostgresService postgresService) =>
            new CategoryRepository(
                postgresService,
                new CategoryEntityValidators()
            );
    }
}
