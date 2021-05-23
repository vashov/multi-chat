using System;
using System.Collections.Generic;

namespace MultiChat.Server.Models
{
    public class Invitation
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public List<User> InvitedUsers { get; set; } = new List<User>();

        public bool IsPermanent { get; set; }

        public DateTimeOffset ExpireAt { get; set; }
    }
}
