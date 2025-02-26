using System.Data;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;

namespace DataAccess.Tests.Unit
{
    public class DictionaryExtensionsTest
    {
        [Fact]
        public void ShouldReturnCorrectValue()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("key", "value");

            string received = dict.GetColumn<string>("key");
            Assert.Equal("value", received);
        }

        [Fact]
        public void ShouldThrowAnException_WhenTypesNotMatch()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("key", "value");

            Assert.Throws<DataException>(() => dict.GetColumn<int>("key"));
        }
    }
}