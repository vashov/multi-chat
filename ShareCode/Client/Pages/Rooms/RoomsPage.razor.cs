using Microsoft.AspNetCore.Components;
using ShareCode.Client.Services.Rooms;
using ShareCode.Shared.Rooms.Create;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareCode.Client.Pages.Rooms
{
    public partial class RoomsPage
    {
        private class CreateRoomModel
        {
            public TimeSpan ChatLiveTime { get; set; }

            [Required(ErrorMessage = "Set your name / pseudonym")]
            [RegularExpression("^[A-Za-z0-9]{3,16}$", ErrorMessage = "Length must be between 3 to 16. \nContains only numbers and latin letters.")]
            public string UserName { get; set; }

            public bool OnlyCreatorCanInvite { get; set; }
        }

        private class ChatLiveTime
        {
            public TimeSpan Time { get; set; }
            public string TimeDisplay { get; set; }
        }

        [Inject]
        private NavigationManager Navigator { get; set; }

        [Inject]
        private IRoomService RoomService { get; set; }

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

            base.OnInitialized();
        }

        private CreateRoomModel Model { get; set; } = new CreateRoomModel();

        private ChatLiveTime DefaultChatLiveTime { get; set; }
            = new ChatLiveTime { Time = TimeSpan.FromMinutes(10), TimeDisplay = "5 mins" };

        private List<ChatLiveTime> ChatLiveTimes { get; set; }

        private async void CreateChatRoom()
        {
            var request = new CreateRequest()
            {
                ChatLiveTime = Model.ChatLiveTime,
                OnlyOwnerCanInvite = Model.OnlyCreatorCanInvite,
                UserName = Model.UserName
            };

            var serviceResult = await RoomService.Create(request);
            if (!serviceResult.IsOk)
            {
                Console.WriteLine(serviceResult.ErrorMsg);
                return;
            }

            Navigator.NavigateTo($"/room/{serviceResult.Result.ToString("N")}");
            
            Console.WriteLine("Room created");
        }

        private void OnCancel()
        {
            Navigator.NavigateTo("/");
        }
    }
}
