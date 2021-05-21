﻿using ShareCode.Shared.Rooms.Create;
using ShareCode.Shared.Rooms.Enter;
using System;
using System.Threading.Tasks;

namespace ShareCode.Client.Services.Rooms
{
    public interface IRoomService
    {
        Task<ServiceResult<CreateResponse>> Create(CreateRequest request);

        Task<ServiceResult<EnterResponse>> Enter(EnterRequest request);
    }
}
