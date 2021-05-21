using Microsoft.AspNetCore.Components;
using ShareCode.Client.Components;
using ShareCode.Client.Services.RoomObserver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        [Parameter]
        public string InviteId { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private EnterChatWindow EnterChatWindowInstance { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            RoomObserver.RoomConnected += RoomObserver_RoomConnected;
        }

        protected override Task OnParametersSetAsync()
        {
            

            return base.OnParametersSetAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (!string.IsNullOrEmpty(InviteId))
            {
                Guid invite = Guid.Parse(InviteId);
                EnterChatWindowInstance.InviteId = invite;
                EnterChatWindowInstance.IsModalVisible = true;
                NavigationManager.NavigateTo("/");
            }

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
                UserId = roomArgs.UserId,
                RoomId = roomArgs.RoomId,
                Name = roomArgs.RoomTopic 
            };

            Items.Add(room);
            StateHasChanged();
        }
    }
}
