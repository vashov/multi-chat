using Microsoft.AspNetCore.Mvc;
using MultiChat.Server.Models;
using MultiChat.Server.Services.Invitations;
using MultiChat.Server.Services.Rooms;
using MultiChat.Shared;
using MultiChat.Shared.Invitations.Create;
using System.Threading.Tasks;

namespace MultiChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly InvitationService _invitationService;

        public InvitationController(
            RoomService roomService,
            InvitationService invitationService)
        {
            _roomService = roomService;
            _invitationService = invitationService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<CreateResponse>>> Create(CreateRequest request)
        {
            if (!await _roomService.CheckUserCanInvite(request.UserId, request.RoomId))
                return OperationResult<CreateResponse>.Error("Can't invite to room");

            var room = await _roomService.Get(request.RoomId);
            Invitation invitaion = await _invitationService.Create(request.UserId, room.Id, request.IsPermanent, room.ExpireAt);

            return OperationResult<CreateResponse>.Ok(new CreateResponse
            {
                InvitationId = invitaion.Id
            });
        }
    }
}
