﻿using System.Text.Json;

namespace ECommerce.Application.src.Utils
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
        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }
    }
}
