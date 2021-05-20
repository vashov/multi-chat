using System;

namespace ShareCode.Server.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid PublicId { get; set; }

        public string Name { get; set; }

        public DateTimeOffset ExpireAt { get; set; }
    }
}
