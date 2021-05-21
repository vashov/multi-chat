using System;

namespace ShareCode.Shared.Rooms.Enter
{
    public class EnterResponse
    {
        public Guid RoomId { get; set; }
        public Guid RoomOwnerPublicId { get; set; }
        public string RoomTopic { get; set; }

        public Guid UserId { get; set; }
        public Guid UserPublicId { get; set; }

        public DateTimeOffset RoomExpireAt { get; set; }

        public bool OnlyOwnerCanInvite { get; set; }
    }
}
