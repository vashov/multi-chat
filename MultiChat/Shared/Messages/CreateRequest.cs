using System;

namespace MultiChat.Shared.Messages
{
    public class CreateRequest
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}
