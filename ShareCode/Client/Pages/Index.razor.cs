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
        private List<RoomObserver.RoomConnectedArgs> Items => RoomObserver.Rooms;

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

            //Items.Add(roomArgs);
            StateHasChanged();
        }
    }
}
