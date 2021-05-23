using MultiChat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiChat.Server.Services.Users
{
    public class UserService : IUserService
    {
        private List<User> Users { get; } = new List<User>();

        public User Create(string name, int color, DateTimeOffset expireAt)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                PublicId = Guid.NewGuid(),
                Name = name,
                Color = color,
                ExpireAt = expireAt
            };

            Users.Add(user);
            return user;
        }

        public User Get(Guid userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }

        public List<User> List(List<Guid> users)
        {
            List<User> resultUsers = Users.Where(u => users.Contains(u.Id ))
                .ToList();

            return resultUsers;
        }

        public User FindByConnectionId(string connectionId)
        {
            var user = Users.FirstOrDefault(u => u.ConnectionId == connectionId);
            return user;
        }

        public bool UpdateConnection(Guid userId, string connectionId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            user.ConnectionId = connectionId;
            return true;
        }

        //public User Get(Guid userId)
        //{
        //    var search = new User { Id = userId };
        //    if (Users.TryGetValue(search, out User user))
        //        return user;

        //    throw new ArgumentException();
        //}
    }
}
