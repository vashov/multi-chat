using Microsoft.AspNetCore.Mvc;
using MultiChat.Server.Models;
using MultiChat.Server.Services.Invitations;
using MultiChat.Server.Services.Rooms;
using MultiChat.Server.Services.Users;
using MultiChat.Shared;
using MultiChat.Shared.Helpers;
using MultiChat.Shared.Rooms.Create;
using MultiChat.Shared.Rooms.Enter;
using System;

namespace MultiChat.Server.Controllers
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
        public ActionResult<OperationResult<CreateResponse>> Create(CreateRequest request)
        {
            TimeSpan lifespan = TimeSpan.Parse(request.ChatLifespan);
            DateTimeOffset expireAt = DateTimeOffset.UtcNow + lifespan;

            ColorEnum color = ColorHelper.RandomColor();
            User user = _userService.Create(request.UserName, (int)color, expireAt);

            Guid roomId = _roomService.Create(user.Id, request.Topic, expireAt, request.OnlyOwnerCanInvite);

            return OperationResult<CreateResponse>.Ok(new CreateResponse
            {
                RoomTopic = request.Topic,
                RoomId = roomId,
                UserId = user.Id,
                UserPublicId = user.PublicId,
                RoomExpireAt = expireAt,
                OnlyOwnerCanInvite = request.OnlyOwnerCanInvite
            });
        }

        [HttpPost("[action]")]
        public ActionResult<OperationResult<EnterResponse>> Enter(EnterRequest request)
        {
            var invitation = _invitationService.Get(request.Invite);
            if (invitation == null)
                return OperationResult<EnterResponse>.Error("Invitation not found.");

            ColorEnum color = ColorHelper.RandomColor();
            User user = _userService.Create(request.UserName, (int)color, invitation.ExpireAt);
            if (invitation.ExpireAt < DateTime.UtcNow)
                return OperationResult<EnterResponse>.Error("Invitation expired.");

            if (!_invitationService.TryUse(user.Id, invitation.Id))
                return OperationResult<EnterResponse>.Error("Can't use invitation.");

            if (!_roomService.TryEnter(user.Id, invitation.RoomId))
                return OperationResult<EnterResponse>.Error("Can't enter to room.");

            Room room = _roomService.Get(invitation.RoomId);

            User owner = _userService.Get(room.OwnerId);

            return OperationResult<EnterResponse>.Ok(new EnterResponse
            {
                RoomId = room.Id,
                RoomOwnerPublicId = owner.PublicId,
                UserId = user.Id,
                UserPublicId = user.PublicId,
                RoomExpireAt = room.ExpireAt,
                RoomTopic = room.Topic,
                OnlyOwnerCanInvite = room.OnlyOwnerCanInvite
            });
        }
    }
}
