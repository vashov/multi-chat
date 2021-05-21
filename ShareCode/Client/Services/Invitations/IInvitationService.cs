using ShareCode.Shared.Invitations.Create;
using System.Threading.Tasks;

namespace ShareCode.Client.Services.Invitations
{
    public interface IInvitationService
    {
        Task<ServiceResult<CreateResponse>> Create(CreateRequest request);
    }
}
