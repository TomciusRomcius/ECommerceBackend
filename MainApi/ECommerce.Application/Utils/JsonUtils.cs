using System.Text.Json;

namespace ECommerce.Application.Utils
{
    public static class JsonUtils
    {
        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }
    }
}
