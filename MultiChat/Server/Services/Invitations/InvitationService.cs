using Microsoft.EntityFrameworkCore;
using MultiChat.Server.Models;
using System;
using System.Threading.Tasks;

namespace MultiChat.Server.Services.Invitations
{
    public class InvitationService
    {
        public InvitationService(MultiChatContext context)
        {
            Context = context;
        }

        public MultiChatContext Context { get; }

        public async Task<Invitation> Get(Guid invitationId)
        {
            var invitation = await Context.Invitations
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            return invitation;
        }

        public async Task<Invitation> Create(Guid userId, Guid roomId, bool isPermanent, DateTimeOffset expireAt)
        {
            var invitation = new Invitation
            {
                OwnerId = userId,
                RoomId = roomId,
                IsPermanent = isPermanent,
                ExpireAt = expireAt
            };

            Context.Invitations.Add(invitation);
            await Context.SaveChangesAsync();
            return invitation;
        }
    }
}
