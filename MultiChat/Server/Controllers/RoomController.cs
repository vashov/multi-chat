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
using System.Threading.Tasks;

namespace MultiChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly UserService _userService;
        private readonly InvitationService _invitationService;

        public RoomController(
            RoomService roomService,
            UserService userService,
            InvitationService invitationService)
        {
            _roomService = roomService;
            _userService = userService;
            _invitationService = invitationService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<CreateResponse>>> Create(CreateRequest request)
        {
            await _roomService.ClearExpired();

            TimeSpan lifespan = TimeSpan.Parse(request.ChatLifespan);
            DateTimeOffset expireAt = DateTimeOffset.UtcNow + lifespan;

            ColorEnum color = ColorHelper.RandomColor();

            Room room = await _roomService.Create(request.UserName, (int)color, request.Topic, expireAt, request.OnlyOwnerCanInvite);

            return OperationResult<CreateResponse>.Ok(new CreateResponse
            {
                RoomTopic = request.Topic,
                RoomId = room.Id,
                UserId = room.OwnerId,
                UserPublicId = room.Owner.PublicId,
                RoomExpireAt = expireAt,
                OnlyOwnerCanInvite = request.OnlyOwnerCanInvite
            });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<EnterResponse>>> Enter(EnterRequest request)
        {
            await _roomService.ClearExpired();

            var invitation = await _invitationService.Get(request.Invite);
            if (invitation == null)
                return OperationResult<EnterResponse>.Error("Invitation not found (or expired).");

            if (invitation.ExpireAt < DateTime.UtcNow)
                return OperationResult<EnterResponse>.Error("Invitation expired.");

            ColorEnum color = ColorHelper.RandomColor();

            User user = await _roomService.TryEnter(request.UserName, (int)color, invitation.RoomId);
            if (user == null)
                return OperationResult<EnterResponse>.Error("Can't enter to room.");

            User owner = await _userService.Get(user.Room.OwnerId);

            return OperationResult<EnterResponse>.Ok(new EnterResponse
            {
                RoomId = user.RoomId,
                RoomOwnerPublicId = owner.PublicId,
                UserId = user.Id,
                UserPublicId = user.PublicId,
                RoomExpireAt = user.ExpireAt,
                RoomTopic = user.Room.Topic,
                OnlyOwnerCanInvite = user.Room.OnlyOwnerCanInvite
            });
        }
    }
}
