using System;

namespace MultiChat.Server.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid PublicId { get; set; }
        public string ConnectionId { get; set; }
        public int Color { get; set; }

        public string Name { get; set; }

        public DateTimeOffset ExpireAt { get; set; }
    }
}
