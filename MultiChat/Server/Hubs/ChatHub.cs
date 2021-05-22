using Microsoft.AspNetCore.SignalR;
using MultiChat.Server.Models;
using MultiChat.Server.Services.Rooms;
using MultiChat.Server.Services.Users;
using MultiChat.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiChat.Server.Hubs
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

            var sendMessage = new SendMessage
            {
                UserName = sender.Name,
                UserPublicId = sender.PublicId,
                Date = DateTime.UtcNow,
                Text = message,
                MessageType = Shared.Messages.SendMessage.MessageTypeEnum.User
            };

            await Clients.Clients(connections).SendAsync("ReceiveMessage", sendMessage);
        }

        public async Task UpdateUserConnection(string user)
        {
            await Task.CompletedTask;

            Guid userId = Guid.Parse(user);

            UserService.UpdateConnection(userId, Context.ConnectionId);
        }

        public async override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            string userIdToken = httpContext.Request.Query["userId"].FirstOrDefault();
            Guid userId = Guid.Parse(userIdToken);
            UserService.UpdateConnection(userId, Context.ConnectionId);

            await NotifyNewUserConnected(userId);

            await base.OnConnectedAsync();
        }

        private async Task NotifyNewUserConnected(Guid userId)
        {
            await NotifyOthers(userId, "connected");
        }

        private async Task NotifyOthers(Guid userId, string text)
        {
            User sender = UserService.Get(userId);

            List<Guid> userIds = RoomService.GetRoommates(userId);
            if (userIds == null)
                return;

            List<User> users = UserService.List(userIds);

            List<string> connections = users
                .Where(u => u.ConnectionId != null && u.Id != userId)
                .Select(u => u.ConnectionId)
                .ToList();

            var sendMessage = new SendMessage
            {
                UserName = sender.Name,
                UserPublicId = Guid.Empty,
                Date = DateTime.UtcNow,
                Text = text,
                MessageType = Shared.Messages.SendMessage.MessageTypeEnum.System
            };

            await Clients.Clients(connections).SendAsync("ReceiveMessage", sendMessage);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = UserService.FindByConnectionId(Context.ConnectionId);
            if (user != null)
            {
                await NotifyOthers(user.Id, "left");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
