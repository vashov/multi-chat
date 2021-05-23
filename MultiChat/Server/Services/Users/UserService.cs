using Microsoft.EntityFrameworkCore;
using MultiChat.Server.Models;
using System;
using System.Threading.Tasks;

namespace MultiChat.Server.Services.Users
{
    public class UserService
    {
        public UserService(MultiChatContext context)
        {
            Context = context;
        }

        public MultiChatContext Context { get; }

        //public async Task<User> Create(string name, int color, DateTimeOffset expireAt)
        //{
        //    var user = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        PublicId = Guid.NewGuid(),
        //        Name = name,
        //        Color = color,
        //        ExpireAt = expireAt
        //    };

        //    Context.Users.Add(user);
        //    await Context.SaveChangesAsync();
        //    return user;
        //}

        public async Task<User> Get(Guid userId)
        {
            var user = await Context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        //public async Task<List<User>> List(List<Guid> users)
        //{
        //    List<User> resultUsers = await Context.Users.Where(u => users.Contains(u.Id))
        //        .ToListAsync();

        //    return resultUsers;
        //}

        public async Task<User> FindByConnectionId(string connectionId)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
            return user;
        }

        public async Task<bool> UpdateConnection(Guid userId, string connectionId)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.ConnectionId = connectionId;
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
