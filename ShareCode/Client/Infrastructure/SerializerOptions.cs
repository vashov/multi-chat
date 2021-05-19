using System.Text.Json;

namespace ShareCode.Client.Infrastructure
{
    public static class SerializerOptions
    {
        private static JsonSerializerOptions _default = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static JsonSerializerOptions Default => _default;
    }
}
