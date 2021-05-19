using System;

namespace ShareCode.Shared.Rooms.Create
{
    public class CreateResponse
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset ExpireAt { get; set; }
    }
}
