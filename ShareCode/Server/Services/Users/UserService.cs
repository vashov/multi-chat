using ShareCode.Server.Models;
using System;
using System.Collections.Generic;

namespace ShareCode.Server.Services.Users
{
    public class UserService : IUserService
    {
        private List<User> Users { get; } = new List<User>();

        public User Create(string name, DateTimeOffset expireAt)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                PublicId = Guid.NewGuid(),
                Name = name,
                ExpireAt = expireAt
            };

            Users.Add(user);
            return user;
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
