using Microsoft.AspNetCore.Mvc;
using MultiChat.Server.Services.Invitations;
using MultiChat.Server.Services.Rooms;
using MultiChat.Server.Services.Users;
using MultiChat.Shared;
using MultiChat.Shared.Invitations.Create;
using System;

namespace MultiChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IInvitationService _invitationService;

        public InvitationController(
            IRoomService roomService,
            IUserService userService,
            IInvitationService invitationService)
        {
            _roomService = roomService;
            _userService = userService;
            _invitationService = invitationService;
        }

        [HttpPost("[action]")]
        public ActionResult<OperationResult<CreateResponse>> Create(CreateRequest request)
        {
            if (!_roomService.CheckUserCanInvite(request.UserId, request.RoomId))
                return OperationResult<CreateResponse>.Error("Can't invite to room");

            var room = _roomService.Get(request.RoomId);
            Guid invitaionId = _invitationService.Create(request.UserId, room.Id, request.IsPermanent, room.ExpireAt);

            return OperationResult<CreateResponse>.Ok(new CreateResponse
            {
                InvitationId = invitaionId
            });
        }
    }
}
