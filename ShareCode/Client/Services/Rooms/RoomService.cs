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

        public async Task<ServiceResult<CreateResponse>> Create(CreateRequest request)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/Room/Create", request);

                if (!response.IsSuccessStatusCode)
                    return ServiceResult<CreateResponse>.Error(response.ReasonPhrase);

                var content = await response.Content.ReadAsStringAsync();
                var createResponse = JsonSerializer.Deserialize<CreateResponse>(content, SerializerOptions.Default);
                return ServiceResult<CreateResponse>.Ok(createResponse);
            }
            catch (Exception e)
            {
                return ServiceResult<CreateResponse>.Error(e.Message);
            }
        }
    }
}
