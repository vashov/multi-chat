using ShareCode.Shared.Rooms.Create;
using System;
using System.Threading.Tasks;

namespace ShareCode.Client.Services.Rooms
{
    public interface IRoomService
    {
        Task<ServiceResult<CreateResponse>> Create(CreateRequest request);
    }
}
