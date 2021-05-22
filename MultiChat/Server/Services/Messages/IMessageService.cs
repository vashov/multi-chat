using System;

namespace MultiChat.Server.Services.Messages
{
    public interface IMessageService
    {
        void Create(Guid roomId, Guid userId, DateTimeOffset expireAt, string text);
    }
}
