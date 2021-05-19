using Microsoft.AspNetCore.Mvc;
using ShareCode.Server.Models;
using ShareCode.Server.Services.Messages;
using ShareCode.Server.Services.Rooms;
using ShareCode.Shared.Messages;

namespace ShareCode.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRoomService _roomService;
        //private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessageController(
            IRoomService roomService, 
            //IUserService userService, 
            IMessageService messageService)
        {
            _roomService = roomService;
            //_userService = userService;
            _messageService = messageService;
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateRequest request)
        {
            Room room = _roomService.GetByUser(request.UserId);
            if (room == null)
                return NotFound();

            _messageService.Create(room.Id, request.UserId, room.ExpireAt, request.Text);

            return Ok();
        }
    }
}
