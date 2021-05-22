using MultiChat.Server.Models;
using System;

namespace MultiChat.Server.Services.Invitations
{
    public interface IInvitationService
    {
        Guid Create(Guid userId, Guid roomId, bool isPermanent, DateTimeOffset expireAt);

        Invitation Get(Guid invitationId);

        bool TryUse(Guid userId, Guid invitationId);
    }
}
