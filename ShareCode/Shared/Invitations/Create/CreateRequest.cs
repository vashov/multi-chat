using System;

namespace ShareCode.Shared.Invitations.Create
{
    public class CreateRequest
    {
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public bool IsPermanent { get; set; }
    }
}
