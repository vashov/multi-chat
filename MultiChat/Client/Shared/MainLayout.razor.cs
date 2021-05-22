using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiChat.Client.Shared
{
    public partial class MainLayout
    {
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
