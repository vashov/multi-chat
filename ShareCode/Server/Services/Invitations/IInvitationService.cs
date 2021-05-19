using ShareCode.Server.Models;
using System;

namespace ShareCode.Server.Services.Invitations
{
    public interface IInvitationService
    {
        Guid Create(Guid userId, Guid roomId, bool isPermanent, DateTimeOffset expireAt);

        Invitation Get(Guid invitationId);

        bool Use(Guid userId, Guid invitationId);
    }
}
