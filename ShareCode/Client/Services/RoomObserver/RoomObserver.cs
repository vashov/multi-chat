using ShareCode.Shared.Rooms.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareCode.Client.Services.RoomObserver
{
    public static class RoomObserver
    {
        public class RoomConnectedArgs : EventArgs
        {
            public Guid RoomId { get; set; }
            public string RoomTopic { get; set; }

            public Guid UserId { get; set; }
            public Guid UserPublicId { get; set; }

            public DateTimeOffset RoomExpireAt { get; set; }
        }

        public static event EventHandler<RoomConnectedArgs> RoomConnected;

        public static void ConnectRoom(RoomConnectedArgs args)
        {
            RoomConnected.Invoke(null, args);
        }
    }
}
