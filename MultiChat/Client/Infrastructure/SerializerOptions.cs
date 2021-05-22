using System.Text.Json;

namespace MultiChat.Client.Infrastructure
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
