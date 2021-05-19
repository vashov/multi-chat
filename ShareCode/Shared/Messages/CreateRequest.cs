using System;

namespace ShareCode.Shared.Messages
{
    public class CreateRequest
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}
