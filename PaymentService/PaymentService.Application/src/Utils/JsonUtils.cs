using System.Text.Json;

namespace PaymentService.Application.src.Utils
{
    public static class JsonUtils
    {
        public static string Serialize(object? obj)
        {
            if (obj == null) return "";
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
