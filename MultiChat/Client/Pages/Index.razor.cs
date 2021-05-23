using Microsoft.AspNetCore.Components;
using MultiChat.Client.Components;
using MultiChat.Client.Services.Notify;
using MultiChat.Client.Services.RoomsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MultiChat.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public RoomsManagerService RoomManager { get; set; }

        [Parameter]
        public string InviteId { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private GlobalNotifyService GlobalNotifyService { get; set; }

        private EnterChatWindow EnterChatWindowInstance { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            RoomManager.RoomConnected += RoomObserver_RoomConnected;
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

        private void CloseChat(Guid roomId)
        {
            var room = RoomManager.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room == null)
                return;

            RoomManager.RemoveRoom(roomId);
            GlobalNotifyService.AddAutoClosingInfoNotification(@$"Room ""{room.RoomTopic}"" expired");
            StateHasChanged();
        }

        private void RoomObserver_RoomConnected(object sender, RoomsManagerService.RoomConnectedArgs roomArgs)
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
