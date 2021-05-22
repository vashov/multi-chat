using MultiChat.Shared.Invitations.Create;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.Invitations
{
    public interface IInvitationService
    {
        Task<ServiceResult<CreateResponse>> Create(CreateRequest request);
    }
}
