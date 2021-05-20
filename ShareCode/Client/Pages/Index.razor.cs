using Microsoft.AspNetCore.Components;
using ShareCode.Client.Services.RoomObserver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;

namespace ShareCode.Client.Pages
{
    public partial class Index
    {
        private class RoomInfo
        {
            public Guid RoomId { get; set; }
            public Guid UserId { get; set; }
            public string Name { get; set; }
        }

        private List<RoomInfo> Items { get; set; } = new List<RoomInfo>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Items.Add(new RoomInfo { Name = "Some room One." });
            //Items.Add(new RoomInfo { Name = "Some room Two." });
            //Items.Add(new RoomInfo { Name = "Some room Three." });
            //Items.Add(new RoomInfo { Name = "Some room Four." });

            RoomObserver.RoomConnected += RoomObserver_RoomConnected;
        }

        private void RoomObserver_RoomConnected(object sender, RoomObserver.RoomConnectedArgs roomArgs)
        {
            //string name;
            //if (Guid.TryParseExact(roomArgs.Room.RoomTopic, "N", out Guid roomId))
            //    name = roomId.ToString();
            //else
            //    name = roomArgs.Topic;

            Console.WriteLine("RoomObserver_RoomConnected");

            var room = new RoomInfo 
            { 
                UserId = roomArgs.Room.UserId,
                RoomId = roomArgs.Room.RoomId,
                Name = roomArgs.Room.RoomTopic 
            };

            Items.Add(room);
            StateHasChanged();
        }
    }
}
