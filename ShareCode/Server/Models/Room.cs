using System;
using System.Collections.Generic;

namespace ShareCode.Server.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public DateTimeOffset ExpireAt { get; set; }

        public bool OnlyOwnerCanInvite { get; set; }

        public List<Guid> Users { get; set; }

        public List<Guid> Messages { get; set; }
    }
}
