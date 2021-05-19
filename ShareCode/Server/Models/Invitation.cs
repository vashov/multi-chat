using System;
using System.Collections.Generic;

namespace ShareCode.Server.Models
{
    public class Invitation
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }
        public Guid RoomId { get; set; }

        public List<Guid> InvitedUsers { get; set; } = new List<Guid>();

        public bool IsPermanent { get; set; }

        public DateTimeOffset ExpireAt { get; set; }
    }
}
