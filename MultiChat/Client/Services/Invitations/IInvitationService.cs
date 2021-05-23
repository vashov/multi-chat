using MultiChat.Shared;
using MultiChat.Shared.Invitations.Create;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.Invitations
{
    public interface IInvitationService
    {
        Task<OperationResult<CreateResponse>> Create(CreateRequest request);
    }
}
