using Microsoft.AspNetCore.Mvc;
using ShareCode.Server.Models;
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
            TimeSpan lifespan = TimeSpan.Parse(request.ChatLifespan);
            DateTimeOffset expireAt = DateTimeOffset.UtcNow + lifespan;

            User user = _userService.Create(request.UserName, expireAt);

            Guid roomId = _roomService.Create(user.Id, request.Topic, expireAt, request.OnlyOwnerCanInvite);

            return new CreateResponse
            {
                RoomTopic = request.Topic,
                RoomId = roomId,
                UserId = user.Id,
                UserPublicId = user.PublicId,
                RoomExpireAt = expireAt,
                OnlyOwnerCanInvite = request.OnlyOwnerCanInvite
            };
        }

        [HttpPost("[action]")]
        public ActionResult<EnterResponse> Enter(EnterRequest request)
        {
            var invitation = _invitationService.Get(request.Invite);
            if (invitation == null)
                return BadRequest("Invitation not found.");

            User user = _userService.Create(request.UserName, invitation.ExpireAt);
            if (!_invitationService.TryUse(user.Id, invitation.Id))
                return BadRequest("Can't use invitation.");

            if (!_roomService.TryEnter(user.Id, invitation.RoomId))
                return BadRequest("Can't enter to room.");

            Room room = _roomService.Get(invitation.RoomId);

            User owner = _userService.Get(room.OwnerId);

            return new EnterResponse
            {
                RoomId = room.Id,
                RoomOwnerPublicId = owner.PublicId,
                UserId = user.Id,
                UserPublicId = user.PublicId,
                RoomExpireAt = room.ExpireAt,
                RoomTopic = room.Topic,
                OnlyOwnerCanInvite = room.OnlyOwnerCanInvite
            };
        }
    }
}
