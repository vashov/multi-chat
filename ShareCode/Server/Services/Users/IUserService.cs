using ShareCode.Server.Models;
using System;
using System.Collections.Generic;

namespace ShareCode.Server.Services.Users
{
    public interface IUserService
    {
        User Create(string name, DateTimeOffset expireAt);

        bool UpdateConnection(Guid userId, string connectionId);

        User Get(Guid userId);
        List<User> List(List<Guid> users);
    }
}
