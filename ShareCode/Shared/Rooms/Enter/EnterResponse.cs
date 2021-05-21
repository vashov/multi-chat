using System;

namespace ShareCode.Shared.Rooms.Enter
{
    public class EnterResponse
    {
        public Guid RoomId { get; set; }
        public string RoomTopic { get; set; }

        public Guid UserId { get; set; }
        public Guid UserPublicId { get; set; }

        public DateTimeOffset RoomExpireAt { get; set; }
    }
}
