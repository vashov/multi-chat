using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MultiChat.Client.Services.RoomsManager;
using MultiChat.Client.Services.Rooms;
using MultiChat.Shared.Rooms.Create;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MultiChat.Client.Services.Notify;

namespace MultiChat.Client.Components
{
    public partial class CreateChatWindow
    {
        private class CreateRoomModel
        {
            private const string _textRegex = "^[A-Za-z0-9]{1,}$";

            [Required]
            [StringLength(maximumLength: 16, MinimumLength = 3)]
            public string Topic { get; set; }

            public string ChatLiveTime { get; set; }

            [Required]
            [StringLength(maximumLength: 16, MinimumLength = 3)]
            [RegularExpression(_textRegex)]
            public string UserName { get; set; }

            public bool OnlyCreatorCanInvite { get; set; }

            public string LengthValidation(string text) => text == null ||
                text.Length < 3 || text.Length > 16 ? "text-danger" : "text-success";

            public string RegexValidation(string text) => text == null || 
                !Regex.IsMatch(text, _textRegex) ? "text-danger" : "text-success";

            public void Clear()
            {
                Topic = "";
                UserName = "";
                OnlyCreatorCanInvite = false;
            }
        }

        private class ChatLiveTime
        {
            public string Time { get; set; }
            public string TimeDisplay { get; set; }
        }

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

        [Inject]
        private IRoomService RoomService { get; set; }

        [Inject]
        private RoomsManagerService RoomsManager { get; set; }

        [Inject]
        private GlobalNotifyService NotifyService { get; set; }

        private CreateRoomModel Model { get; set; } = new CreateRoomModel();

        private ChatLiveTime DefaultChatLiveTime { get; set; }
            = new ChatLiveTime { Time = TimeSpan.FromMinutes(10).ToString(), TimeDisplay = "10 mins" };

        private List<ChatLiveTime> ChatLiveTimes { get; set; }

        private EditContext Context { get; set; }

        protected override void OnInitialized()
        {
            ChatLiveTimes = new List<ChatLiveTime>
            {
                new ChatLiveTime { Time = TimeSpan.FromMinutes(1).ToString(), TimeDisplay = "1 min" },
                new ChatLiveTime { Time = TimeSpan.FromMinutes(5).ToString(), TimeDisplay = "5 mins" },
                DefaultChatLiveTime,
                new ChatLiveTime { Time = TimeSpan.FromMinutes(30).ToString(), TimeDisplay = "30 mins" },
                new ChatLiveTime { Time = TimeSpan.FromHours(1).ToString(), TimeDisplay = "1 hour" },
                //new ChatLiveTime { Time = TimeSpan.FromDays(1), TimeDisplay = "1 day" },
            };

            Model.ChatLiveTime = DefaultChatLiveTime.Time;
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
                ChatLifespan = Model.ChatLiveTime,
                OnlyOwnerCanInvite = Model.OnlyCreatorCanInvite,
                UserName = Model.UserName,
            };

            var serviceResult = await RoomService.Create(request);
            if (!serviceResult.IsOk)
            {
                NotifyService.AddAutoClosingErrorNotification(serviceResult.ErrorMsg);
                return;
            }

            RoomsManager.ConnectRoom(new RoomsManagerService.RoomConnectedArgs
            {
                RoomId = serviceResult.Result.RoomId,
                RoomOwnerPublicId = serviceResult.Result.UserPublicId,
                RoomTopic = serviceResult.Result.RoomTopic,
                UserId = serviceResult.Result.UserId,
                UserPublicId = serviceResult.Result.UserPublicId,
                RoomExpireAt = serviceResult.Result.RoomExpireAt,
                OnlyOwnerCanInvite = serviceResult.Result.OnlyOwnerCanInvite
            });

            IsModalVisible = false;
            Model.Clear();

            Console.WriteLine("Room created");
        }
    }
}
