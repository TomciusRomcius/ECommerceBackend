using System.Data;
using ECommerce.Infrastructure.src.Utils;

namespace ECommerce.Infrastructure.Tests.Unit;

public class DictionaryExtensionsTest
{
    [Fact]
    public void ShouldReturnCorrectValue()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("key", "value");

        var received = dict.GetColumn<string>("key");
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