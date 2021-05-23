using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiChat.Client.Services.RoomsManager
{
    public class RoomsManagerService
    {
        public class RoomConnectedArgs : EventArgs
        {
            public Guid RoomId { get; set; }
            public Guid RoomOwnerPublicId { get; set; }
            public string RoomTopic { get; set; }

            public Guid UserId { get; set; }
            public Guid UserPublicId { get; set; }

            public DateTimeOffset RoomExpireAt { get; set; }

            public bool OnlyOwnerCanInvite { get; set; }
        }

        public List<RoomConnectedArgs> Rooms { get; } = new List<RoomConnectedArgs>();

        public event EventHandler<RoomConnectedArgs> RoomConnected;

        public void ConnectRoom(RoomConnectedArgs args)
        {
            Rooms.Add(args);
            RoomConnected.Invoke(null, args);
        }

        public bool AlreadyInRoom(Guid roomId)
        {
            return Rooms.Any(r => r.RoomId == roomId);
        }

        public void RemoveRoom(Guid roomId)
        {
            Rooms.RemoveAll(r => r.RoomId == roomId);
        }
    }
}
