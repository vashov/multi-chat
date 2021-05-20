using ShareCode.Server.Models;
using System;

namespace ShareCode.Server.Services.Rooms
{
    public interface IRoomService
    {
        Guid Create(Guid ownerId, string topic, DateTimeOffset expireAt, bool onlyOwnerCanInvite);
        Room Get(Guid roomId);

        bool CheckUserCanInvite(Guid userId, Guid roomId);

        bool TryEnter(Guid userId, Guid roomId);
        Room GetByUser(Guid userId);
    }
}
