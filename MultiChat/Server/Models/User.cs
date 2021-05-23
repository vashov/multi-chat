using System;
using System.Collections.Generic;

namespace MultiChat.Server.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid PublicId { get; set; }
        public string ConnectionId { get; set; }
        public int Color { get; set; }
        
        public Guid? InvitationId { get; set; }
        public Invitation Invitation { get; set; }

        public string Name { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public Guid? RoomCreatedId { get; set; }
        public Room RoomCreated { get; set; }

        public DateTimeOffset ExpireAt { get; set; }

        public List<Invitation> InvitationsCreated { get; set; }
    }
}
