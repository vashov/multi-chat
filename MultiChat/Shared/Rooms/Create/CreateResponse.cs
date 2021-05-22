using System;

namespace MultiChat.Shared.Rooms.Create
{
    public class CreateResponse
    {
        public Guid RoomId { get; set; }
        public string RoomTopic { get; set; }

        public Guid UserId { get; set; }
        public Guid UserPublicId { get; set; }

        public DateTimeOffset RoomExpireAt { get; set; }

        public bool OnlyOwnerCanInvite { get; set; }
    }
}
