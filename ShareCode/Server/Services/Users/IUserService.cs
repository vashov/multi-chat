﻿using ShareCode.Server.Models;
using System;

namespace ShareCode.Server.Services.Users
{
    public interface IUserService
    {
        User Create(string name, DateTimeOffset expireAt);

        //User Get(Guid userId);
    }
}