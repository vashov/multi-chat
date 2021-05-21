using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareCode.Client.Shared
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

        private ShareCode.Client.Components.CreateChatWindow CreateChatWindowInstance { get; set; }
        private ShareCode.Client.Components.EnterChatWindow EnterChatWindowInstance { get; set; }
    }
}
