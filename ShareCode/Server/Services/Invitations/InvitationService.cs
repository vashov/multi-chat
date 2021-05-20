using ShareCode.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareCode.Server.Services.Invitations
{
    public class InvitationService : IInvitationService
    {
        private List<Invitation> Invitations { get; } = new List<Invitation>();

        public bool TryUse(Guid userId, Guid invitationId)
        {
            var invitation = Invitations
                .FirstOrDefault(i => i.Id == invitationId && i.ExpireAt > DateTimeOffset.UtcNow);

            if (invitation == null)
                return false;

            if (!invitation.IsPermanent && invitation.InvitedUsers.Any())
                return false;

            invitation.InvitedUsers.Add(userId);
            return true;
        }

        public Invitation Get(Guid invitationId)
        {
            var invitation = Invitations.FirstOrDefault(i => i.Id == invitationId);

            return invitation;
        }

        public Guid Create(Guid userId, Guid roomId, bool isPermanent, DateTimeOffset expireAt)
        {
            var invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                OwnerId = userId,
                RoomId = roomId,
                IsPermanent = isPermanent,
                ExpireAt = expireAt
            };

            Invitations.Add(invitation);
            return invitation.Id;
        }
    }
}
