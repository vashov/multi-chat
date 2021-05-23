using Microsoft.EntityFrameworkCore;
using MultiChat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiChat.Server.Services.Rooms
{
    public class RoomService
    {
        public MultiChatContext Context { get; }

        public RoomService(MultiChatContext context)
        {
            Context = context;
        }

        public async Task<bool> CheckUserCanInvite(Guid userId, Guid roomId)
        {
            var room = await Context.Rooms
                .AsNoTracking()
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                return false;

            if (room.OnlyOwnerCanInvite && room.OwnerId != userId)
                return false;

            if (!room.Users.Any(u => u.Id == userId))
                return false;

            return true;
        }

        public async Task<Room> Create(string username, int usercolor, string topic, DateTimeOffset expireAt, bool onlyOwnerCanInvite)
        {
            var user = new User
            {
                PublicId = Guid.NewGuid(),
                Name = username,
                Color = usercolor,
                ExpireAt = expireAt
            };

            var room = new Room
            {
                Id = Guid.NewGuid(),
                Topic = topic,
                Owner = user,
                OnlyOwnerCanInvite = onlyOwnerCanInvite,
                ExpireAt = expireAt,
                Users = new List<User> { user }
            };

            Context.Rooms.Add(room);
            await Context.SaveChangesAsync();

            return room;
        }

        public async Task<User> TryEnter(string username, int usercolor, Guid roomId)
        {
            //TODO: Add Timestamp for optimistic concurrency controll
            Room room = await Context.Rooms
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
                return null;

            var user = new User
            {
                PublicId = Guid.NewGuid(),
                Name = username,
                Color = usercolor,
                ExpireAt = room.ExpireAt,
                Room = room,
            };

            room.Users.Add(user);
            await Context.SaveChangesAsync();
            return user;
        }

        public async Task<Room> Get(Guid roomId)
        {
            var room = await Context.Rooms
                .AsNoTracking()
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            return room;
        }

        public async Task<Room> GetByUser(Guid userId)
        {
            User user = await Context.Users
                .AsNoTracking()
                .Include(u => u.Room)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user.Room;
        }

        public async Task<List<User>> GetRoommates(Guid userId)
        {
            Room room = await Context.Rooms
                .AsNoTracking()
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Users.Any(u => u.Id == userId));
            if (room == null)
                return null;

            return room.Users;
        }
    }
}
