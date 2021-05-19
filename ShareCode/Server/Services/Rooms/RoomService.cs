using ShareCode.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareCode.Server.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private List<Room> Rooms { get; } = new List<Room>();

        public bool CheckUserCanInvite(Guid userId, Guid roomId)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null)
                return false;

            if (room.OnlyOwnerCanInvite && room.OwnerId != userId)
                return false;

            if (!room.Users.Contains(userId))
                return false;

            return true;
        }

        public Guid Create(Guid ownerId, DateTimeOffset expireAt, bool onlyOwnerCanInvite)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                OwnerId = ownerId,
                OnlyOwnerCanInvite = onlyOwnerCanInvite,
                ExpireAt = expireAt,
                Users = new List<Guid> { ownerId }
            };

            Rooms.Add(room);

            return room.Id;
        }

        public bool Enter(Guid userId, Guid roomId)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null)
                return false;

            if (room.Users.Contains(userId))
                return false;

            room.Users.Add(userId);
            return true;
        }

        public Room Get(Guid roomId)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            return room;
        }

        public Room GetByUser(Guid userId)
        {
            Room room = Rooms.FirstOrDefault(r => r.Users.Contains(userId));
            return room;
        }
    }
}
