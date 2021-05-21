using Microsoft.AspNetCore.SignalR;
using ShareCode.Server.Models;
using ShareCode.Server.Services.Rooms;
using ShareCode.Server.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareCode.Server.Hubs
{
    public class ChatHub : Hub
    {
        private IUserService UserService { get; }
        public IRoomService RoomService { get; }

        public ChatHub(IUserService userService, IRoomService roomService)
        {
            UserService = userService;
            RoomService = roomService;
        }

        public async Task SendMessage(string user, string message)
        {
            string connectionId = Context.ConnectionId;

            Guid userId = Guid.Parse(user);
            User sender = UserService.Get(userId);
            if (sender == null)
                return;

            List<Guid> userIds = RoomService.GetRoommates(userId);
            if (userIds == null)
                return;

            List<User> users = UserService.List(userIds);

            List<string> connections = users
                .Where(u => u.ConnectionId != null)
                .Select(u => u.ConnectionId)
                .ToList();

            await Clients.Clients(connections).SendAsync("ReceiveMessage", sender.Name, message);
        }

        //public async Task EnterToRoom(string room, string user)
        //{
        //    Guid roomId = Guid.Parse(room);
        //    Guid userId = Guid.Parse(user);
        //}

        public async Task UpdateUserConnection(string user)
        {
            await Task.CompletedTask;

            Guid userId = Guid.Parse(user);

            UserService.UpdateConnection(userId, Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            //string name = Context.User.Identity.Name;
            var httpContext = Context.GetHttpContext();
            string userIdToken = httpContext.Request.Query["userId"].FirstOrDefault();
            Guid userId = Guid.Parse(userIdToken);
            UserService.UpdateConnection(userId, Context.ConnectionId);

            //_connections.Add(name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
