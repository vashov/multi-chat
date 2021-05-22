using Microsoft.AspNetCore.Components;
using MultiChat.Client.Services.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MultiChat.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private GlobalNotifyService NotifyService { get; set; }

        private void BeginRoomCreating()
        {
            CreateChatWindowInstance.IsModalVisible = true;
        }

        private void UseInvite()
        {
            EnterChatWindowInstance.IsModalVisible = true;
        }

        private MultiChat.Client.Components.CreateChatWindow CreateChatWindowInstance { get; set; }
        private MultiChat.Client.Components.EnterChatWindow EnterChatWindowInstance { get; set; }
    }
}
