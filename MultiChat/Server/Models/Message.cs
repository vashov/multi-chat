using System;

namespace MultiChat.Server.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid RommId { get; set; }
        public Guid OwnerId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpireAt { get; set; }
    }
}
