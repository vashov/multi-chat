using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ShareCode.Client.Services.RoomObserver;
using ShareCode.Client.Services.Rooms;
using ShareCode.Shared.Rooms.Create;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ShareCode.Client.Components
{
    public partial class CreateChatWindow
    {
        private bool _isModalVisible;
        public bool IsModalVisible 
        { 
            get => _isModalVisible;
            set
            {
                _isModalVisible = value;
                StateHasChanged();
            } 
        }

        private class CreateRoomModel
        {
            private const string TextRegex = "^[A-Za-z0-9]{1,}$";

            [Required]
            [StringLength(maximumLength: 16, MinimumLength = 3)]
            public string Topic { get; set; }

            public TimeSpan ChatLiveTime { get; set; }

            [Required]
            [StringLength(maximumLength: 16, MinimumLength = 3)]
            [RegularExpression(TextRegex)]
            public string UserName { get; set; }

            public bool OnlyCreatorCanInvite { get; set; }

            public string LengthValidation(string text) => text == null ||
                text.Length < 3 || text.Length > 16 ? "text-danger" : "text-success";

            public string RegexValidation(string text) => text == null || 
                !Regex.IsMatch(text, TextRegex) ? "text-danger" : "text-success";
        }

        private class ChatLiveTime
        {
            public TimeSpan Time { get; set; }
            public string TimeDisplay { get; set; }
        }

        [Inject]
        private IRoomService RoomService { get; set; }

        private CreateRoomModel Model { get; set; } = new CreateRoomModel();

        private ChatLiveTime DefaultChatLiveTime { get; set; }
            = new ChatLiveTime { Time = TimeSpan.FromMinutes(10), TimeDisplay = "5 mins" };

        private List<ChatLiveTime> ChatLiveTimes { get; set; }

        private EditContext Context { get; set; }
        protected override void OnInitialized()
        {
            ChatLiveTimes = new List<ChatLiveTime>
            {
                new ChatLiveTime { Time = TimeSpan.FromMinutes(1), TimeDisplay = "1 min" },
                DefaultChatLiveTime,
                new ChatLiveTime { Time = TimeSpan.FromMinutes(5), TimeDisplay = "10 mins" },
                new ChatLiveTime { Time = TimeSpan.FromMinutes(30), TimeDisplay = "30 mins" },
                new ChatLiveTime { Time = TimeSpan.FromHours(1), TimeDisplay = "1 hour" },
                //new ChatLiveTime { Time = TimeSpan.FromDays(1), TimeDisplay = "1 day" },
            };

            Context = new EditContext(Model);

            base.OnInitialized();
        }

        private async void CreateChatRoom()
        {
            if (!Context.Validate())
                return;

            var request = new CreateRequest()
            {
                Topic = Model.Topic,
                ChatLiveTime = Model.ChatLiveTime,
                OnlyOwnerCanInvite = Model.OnlyCreatorCanInvite,
                UserName = Model.UserName,
            };

            var serviceResult = await RoomService.Create(request);
            if (!serviceResult.IsOk)
            {
                Console.WriteLine(serviceResult.ErrorMsg);
                return;
            }

            RoomObserver.ConnectRoom(new RoomObserver.RoomConnectedArgs
            {
                Room = serviceResult.Result
            });
            //Navigator.NavigateTo($"/room/{serviceResult.Result:N}");

            IsModalVisible = false;
            Console.WriteLine("Room created");
        }
    }
}
