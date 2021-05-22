using MultiChat.Shared.Rooms.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiChat.Client.Services.RoomObserver
{
    public static class RoomObserver
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

        public static List<RoomConnectedArgs> Rooms { get; } = new List<RoomConnectedArgs>();

        public static event EventHandler<RoomConnectedArgs> RoomConnected;

        public static void ConnectRoom(RoomConnectedArgs args)
        {
            Rooms.Add(args);
            RoomConnected.Invoke(null, args);
        }

        public static bool AlreadyInRoom(Guid roomId)
        {
            return Rooms.Any(r => r.RoomId == roomId);
        }
    }
}
