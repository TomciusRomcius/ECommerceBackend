using ECommerce.Common.Services;
using ECommerce.Manufacturers;
using Moq;
using Xunit;

namespace ECommerce.Tests
{
    public class ManufacturerServiceTest
    {
        private readonly Mock<IPostgresService> _postgresService;
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerServiceTest()
        {
            _postgresService = new Mock<IPostgresService>();
            _manufacturerService = new ManufacturerService(_postgresService.Object);
        }

        [Fact]
        public async Task GetAllManufacturers_ShouldReturnAllManufacturers()
        {
            int id1 = 1;
            int id2 = 2;
            string name1 = "Category 1";
            string name2 = "Category 2";

            List<Dictionary<string, object>> returnValue = new List<Dictionary<string, object>> {

                new Dictionary<string, object>
                {
                    ["manufacturerid"] = id1,
                    ["name"] = name1,
                },
                new Dictionary<string, object>
                {
                    ["manufacturerid"] = id2,
                    ["name"] = name2,
                }
            };

            _postgresService.Setup(postgres => postgres.ExecuteAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>())).ReturnsAsync(returnValue);

            List<ManufacturerModel> result = await _manufacturerService.GetAllManufacturers();

            Assert.Equal(returnValue.Count, result.Count);

            Assert.Equal(id1, result[0].ManufacturerId);
            Assert.Equal(id2, result[1].ManufacturerId);

            Assert.Equal(name1, result[0].Name);
            Assert.Equal(name2, result[1].Name);
        }
    }
}
