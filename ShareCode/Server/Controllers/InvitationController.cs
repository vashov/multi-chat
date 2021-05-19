using Microsoft.AspNetCore.Mvc;
using ShareCode.Server.Services.Invitations;
using ShareCode.Server.Services.Rooms;
using ShareCode.Server.Services.Users;
using ShareCode.Shared.Invitations.Create;
using System;

namespace ShareCode.Server.Controllers
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
        public ActionResult<CreateResponse> Create(CreateRequest request)
        {
            if (!_roomService.CheckUserCanInvite(request.UserId, request.RoomId))
                return BadRequest("Can't invite to room");

            var room = _roomService.Get(request.RoomId);
            Guid invitaionId = _invitationService.Create(request.UserId, room.Id, request.IsPermanent, room.ExpireAt);

            return new CreateResponse
            {
                InvitationId = invitaionId
            };
        }
    }
}
