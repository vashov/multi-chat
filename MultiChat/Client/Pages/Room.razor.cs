using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiChat.Client.Pages
{
    public partial class Room
    {
        private Guid RoomId { get; set; }

        private string _id;
        //[Parameter]
        public string Id 
        { 
            get => _id;
            set
            {
                _id = value;
                if (Guid.TryParseExact(_id, "N", out Guid roomId))
                    RoomId = roomId;
            } 
        }
    }
}
