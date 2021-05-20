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
            public CreateResponse Room { get; set; }
        }

        public static event EventHandler<RoomConnectedArgs> RoomConnected;

        public static void ConnectRoom(RoomConnectedArgs args)
        {
            RoomConnected.Invoke(null, args);
        }
    }
}
