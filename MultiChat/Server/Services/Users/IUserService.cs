using MultiChat.Server.Models;
using System;
using System.Collections.Generic;

namespace MultiChat.Server.Services.Users
{
    public interface IUserService
    {
        User Create(string name, DateTimeOffset expireAt);

        bool UpdateConnection(Guid userId, string connectionId);

        User Get(Guid userId);
        List<User> List(List<Guid> users);

        User FindByConnectionId(string connectionId);
    }
}
