using ShareCode.Client.Infrastructure;
using ShareCode.Shared.Rooms.Create;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShareCode.Client.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly HttpClient _client;

        public RoomService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ServiceResult<Guid>> Create(CreateRequest request)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("Room/Create", request);

                if (!response.IsSuccessStatusCode)
                    return ServiceResult<Guid>.Error(response.ReasonPhrase);

                var content = await response.Content.ReadAsStringAsync();
                var roomGuid = JsonSerializer.Deserialize<CreateResponse>(content, SerializerOptions.Default);
                return ServiceResult<Guid>.Ok(roomGuid.RoomId);
            }
            catch (Exception e)
            {
                return ServiceResult<Guid>.Error(e.Message);
            }
        }
    }
}
