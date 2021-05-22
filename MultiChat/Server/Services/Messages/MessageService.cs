using MultiChat.Server.Models;
using System;
using System.Collections.Generic;

namespace MultiChat.Server.Services.Messages
{
    public class MessageService : IMessageService
    {
        private HashSet<Message> Messages { get; } = new HashSet<Message>();

        public void Create(Guid roomId, Guid userId, DateTimeOffset expireAt, string text)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                RommId = roomId,
                OwnerId = userId,
                Text = text,
                CreatedAt = DateTimeOffset.UtcNow,
                ExpireAt = expireAt
            };

            Messages.Add(message);
        }
    }
}
