using MultiChat.Shared;
using MultiChat.Shared.Rooms.Create;
using MultiChat.Shared.Rooms.Enter;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.Rooms
{
    public interface IRoomService
    {
        Task<OperationResult<CreateResponse>> Create(CreateRequest request);

        Task<OperationResult<EnterResponse>> Enter(EnterRequest request);
    }
}
