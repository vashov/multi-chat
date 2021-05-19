using Microsoft.AspNetCore.Mvc;
using ShareCode.Server.Services.Invitations;
using ShareCode.Server.Services.Rooms;
using ShareCode.Server.Services.Users;
using ShareCode.Shared.Rooms.Create;
using ShareCode.Shared.Rooms.Enter;
using System;

namespace ShareCode.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IInvitationService _invitationService;

        public RoomController(
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
            DateTimeOffset expireAt = DateTimeOffset.UtcNow + request.ChatLiveTime;

            Guid userId = _userService.Create(request.UserName, expireAt);

            Guid roomId = _roomService.Create(userId, expireAt, request.OnlyOwnerCanInvite);

            return new CreateResponse
            {
                RoomId = roomId,
                UserId = userId,
                ExpireAt = expireAt
            };
        }

        [HttpPost("[action]")]
        public ActionResult<EnterResponse> Enter(EnterRequest request)
        {
            if (!Guid.TryParseExact(request.Invite, "N", out Guid invitationId))
                return BadRequest("Wrong invitaion id.");

            var invitation = _invitationService.Get(invitationId);
            if (invitation == null)
                return BadRequest("Invitation not found.");

            Guid userId = _userService.Create(request.UserName, invitation.ExpireAt);
            if (!_invitationService.Use(userId, invitation.Id))
                return BadRequest("Can't use invitation.");

            if (!_roomService.Enter(userId, invitation.RoomId))
                return BadRequest("Can't enter to room.");

            return new EnterResponse
            {
                RoomId = invitation.RoomId,
                UserId = userId,
            };
        }
    }
}
