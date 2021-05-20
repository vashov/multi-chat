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
            //V = !V;
            CreateChatWindowInstance.IsModalVisible = true;
        }

        public bool V { get; set; } = false;
        ShareCode.Client.Components.CreateChatWindow CreateChatWindowInstance { get; set; }
    }
}
