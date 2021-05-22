using MultiChat.Shared.Rooms.Create;
using MultiChat.Shared.Rooms.Enter;
using System;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.Rooms
{
    public interface IRoomService
    {
        Task<ServiceResult<CreateResponse>> Create(CreateRequest request);

        Task<ServiceResult<EnterResponse>> Enter(EnterRequest request);
    }
}
