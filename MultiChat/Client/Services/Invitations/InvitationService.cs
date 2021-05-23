using MultiChat.Client.Infrastructure;
using MultiChat.Shared;
using MultiChat.Shared.Invitations.Create;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.Invitations
{
    public class InvitationService : IInvitationService
    {
        private readonly HttpClient _client;

        public InvitationService(HttpClient client)
        {
            _client = client;
        }

        public async Task<OperationResult<CreateResponse>> Create(CreateRequest request)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/Invitation/Create", request);

                if (!response.IsSuccessStatusCode)
                    return OperationResult<CreateResponse>.Error(response.ReasonPhrase);

                var content = await response.Content.ReadAsStringAsync();
                var createResponse = JsonSerializer.Deserialize<OperationResult<CreateResponse>>(content, SerializerOptions.Default);
                if (!createResponse.IsOk)
                {
                    return OperationResult<CreateResponse>.Error(createResponse.ErrorMsg);
                }
                return OperationResult<CreateResponse>.Ok(createResponse.Result);
            }
            catch (Exception e)
            {
                return OperationResult<CreateResponse>.Error(e.Message);
            }
        }
    }
}
