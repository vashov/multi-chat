using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ShareCode.Client.Services.RoomObserver;
using ShareCode.Client.Services.Rooms;
using ShareCode.Shared.Rooms.Enter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShareCode.Client.Components
{
    public partial class EnterChatWindow
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

        private class EnterRoomModel
        {
            public const string NameRegex = "^[A-Za-z0-9]{1,}$";

            public string InviteLink { get; set; }

            [Required]
            [StringLength(maximumLength: 16, MinimumLength = 3)]
            [RegularExpression(NameRegex)]
            public string UserName { get; set; }

            public string LengthValidation(string text) => text == null ||
                text.Length < 3 || text.Length > 16 ? "text-danger" : "text-success";

            public string RegexValidation(string text, string pattern) => text == null ||
                !Regex.IsMatch(text, pattern) ? "text-danger" : "text-success";

            public string LinkValidation(string text) 
            {
                if (TryGetInviteId(text, out _))
                    return "text-success";
                return "text-danger";
            }
        }

        private static bool TryGetInviteId(string text, out Guid inviteId)
        {
            inviteId = default;

            if (text == null)
                return false;

            string temp = text.Split('/').LastOrDefault();
            return Guid.TryParse(temp, out inviteId);
        }

        [Inject]
        private IRoomService RoomService { get; set; }

        [Parameter]
        public bool ShowInviteLinkInput { get; set; }

        private EnterRoomModel Model { get; set; } = new EnterRoomModel();

        private EditContext Context { get; set; }

        public Guid InviteId { get; set; }

        protected override void OnInitialized()
        {
            Context = new EditContext(Model);

            base.OnInitialized();
        }

        private async void EnterChatRoom()
        {
            if (!Context.Validate())
                return;

            if (ShowInviteLinkInput)
            {
                if (!TryGetInviteId(Model.InviteLink, out Guid inviteId))
                    return;

                InviteId = inviteId;
            }

            var request = new EnterRequest()
            {
                Invite = InviteId,
                UserName = Model.UserName,
            };

            var serviceResult = await RoomService.Enter(request);
            if (!serviceResult.IsOk)
            {
                Console.WriteLine(serviceResult.ErrorMsg);
                return;
            }

            if (RoomObserver.AlreadyInRoom(serviceResult.Result.RoomId))
            {
                Console.WriteLine("You are already in room.");
                return;
            }

            RoomObserver.ConnectRoom(new RoomObserver.RoomConnectedArgs
            {
                RoomId = serviceResult.Result.RoomId,
                RoomOwnerPublicId = serviceResult.Result.RoomOwnerPublicId,
                RoomTopic = serviceResult.Result.RoomTopic,
                UserId = serviceResult.Result.UserId,
                UserPublicId = serviceResult.Result.UserPublicId,
                RoomExpireAt = serviceResult.Result.RoomExpireAt,
                OnlyOwnerCanInvite = serviceResult.Result.OnlyOwnerCanInvite
            });

            IsModalVisible = false;
            Console.WriteLine("Room entered");
        }
    }
}
