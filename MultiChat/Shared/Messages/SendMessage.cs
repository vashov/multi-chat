using System;

namespace MultiChat.Shared.Messages
{
    public class SendMessage
    {
        public enum MessageTypeEnum
        {
            User = 0,
            System = 1
        }

        public string UserName { get; set; }
        public Guid UserPublicId { get; set; }
        public int UserColor { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public MessageTypeEnum MessageType { get; set; }
    }
}
