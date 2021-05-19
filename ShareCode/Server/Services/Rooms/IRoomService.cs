using ShareCode.Server.Models;
using System;

namespace ShareCode.Server.Services.Rooms
{
    public interface IRoomService
    {
        Guid Create(Guid ownerId, DateTimeOffset expireAt, bool onlyOwnerCanInvite);
        Room Get(Guid roomId);

        bool CheckUserCanInvite(Guid userId, Guid roomId);

        bool Enter(Guid userId, Guid roomId);
        Room GetByUser(Guid userId);
    }
}
