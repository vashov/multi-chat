using MultiChat.Server.Models;
using System;
using System.Collections.Generic;

namespace MultiChat.Server.Services.Rooms
{
    public interface IRoomService
    {
        Guid Create(Guid ownerId, string topic, DateTimeOffset expireAt, bool onlyOwnerCanInvite);
        Room Get(Guid roomId);

        bool CheckUserCanInvite(Guid userId, Guid roomId);

        bool TryEnter(Guid userId, Guid roomId);
        Room GetByUser(Guid userId);

        List<Guid> GetRoommates(Guid userId);
    }
}
