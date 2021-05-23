using MultiChat.Client.Infrastructure;
using MultiChat.Shared;
using MultiChat.Shared.Rooms.Create;
using MultiChat.Shared.Rooms.Enter;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly HttpClient _client;

        public RoomService(HttpClient client)
        {
            _client = client;
        }

        public async Task<OperationResult<CreateResponse>> Create(CreateRequest request)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/Room/Create", request);

                if (!response.IsSuccessStatusCode)
                    return OperationResult<CreateResponse>.Error(response.ReasonPhrase);

                var content = await response.Content.ReadAsStringAsync();
                var createResponse = JsonSerializer.Deserialize<OperationResult<CreateResponse>>(content, SerializerOptions.Default);
                if (!createResponse.IsOk)
                    return OperationResult<CreateResponse>.Error(createResponse.ErrorMsg);

                return OperationResult<CreateResponse>.Ok(createResponse.Result);
            }
            catch (Exception e)
            {
                return OperationResult<CreateResponse>.Error(e.Message);
            }
        }

        public async Task<OperationResult<EnterResponse>> Enter(EnterRequest request)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/Room/Enter", request);

                if (!response.IsSuccessStatusCode)
                    return OperationResult<EnterResponse>.Error(response.ReasonPhrase);

                var content = await response.Content.ReadAsStringAsync();
                var enterResponse = JsonSerializer.Deserialize<OperationResult<EnterResponse>>(content, SerializerOptions.Default);
                if (!enterResponse.IsOk)
                    return OperationResult<EnterResponse>.Error(enterResponse.ErrorMsg);

                return OperationResult<EnterResponse>.Ok(enterResponse.Result);
            }
            catch (Exception e)
            {
                return OperationResult<EnterResponse>.Error(e.Message);
            }
        }
    }
}
