using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MultiChat.Client.Services.Clipboard;
using MultiChat.Client.Services.Invitations;
using MultiChat.Shared.Invitations.Create;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MultiChat.Client.Components
{
    public partial class Chat : IAsyncDisposable
    {
        private class Message
        {
            public string Sender { get; set; }
            public Guid UserPublicId { get; set; }
            public string Text { get; set; }
            public DateTime Date { get; set; }
        }

        [Parameter]
        public Guid UserId { get; set; }

        [Parameter]
        public Guid UserPublicId { get; set; }

        [Parameter]
        public Guid RoomId { get; set; }

        [Parameter]
        public Guid RoomOwnerPublicId { get; set; }

        [Parameter]
        public DateTimeOffset ExpireAt { get; set; }

        [Parameter]
        public bool OnlyOwnerCanInvite { get; set; }

        public bool CanInvite => !OnlyOwnerCanInvite || UserPublicId == RoomOwnerPublicId;

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ClipboardService ClipboardService { get; set; }

        [Inject]
        private IInvitationService InvitationService { get; set; }

        private System.Timers.Timer _timer; 

        private string _timerDisplay;
        private string TimerDisplay
        {
            get => _timerDisplay;
            set
            {
                _timerDisplay = value;
                StateHasChanged();
            }
        }

        public TelerikNotification NotificationReference { get; set; }

        private     HubConnection hubConnection;
        private ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        private int UsersCount => 0;

        private string _messageInput;
        private string MessageInput 
        { 
            get => _messageInput;
            set
            {
                value = value?.Replace('\n', ' ');
                int len = (value?.Length ?? 0) < AllowedLength ? value?.Length ?? 0 : AllowedLength;
                _messageInput = value?.Substring(0, len);
                MessageCounter = GetMessageCounter();
            }
        }

        private const int AllowedLength = 128;

        private string MessageCounterClass => (MessageInput?.Length ?? 0) >= AllowedLength ? "text-danger" : "text-success";

        private string MessageCounter { get; set; }
        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri($"/hub/chat?userId={UserId}"))
                .WithAutomaticReconnect()
                .Build();

            hubConnection.Reconnected += HubConnection_Reconnected;

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";

                var messageInfo = new Message
                {
                    Sender = user,
                    Text = message,
                    Date = DateTime.UtcNow
                };

                Messages.Add(messageInfo);
                StateHasChanged();
            });

            MessageCounter = GetMessageCounter();

            await hubConnection.StartAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            TimerDisplay = GetLeftTime().ToString(@"hh\:mm\:ss");

            _timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += (e, o) =>
            {
                TimeSpan lefttime = GetLeftTime();
                TimerDisplay = lefttime.ToString(@"hh\:mm\:ss");
            };
            _timer.Start();
        }

        private TimeSpan GetLeftTime()
        {
            TimeSpan lefttime = ExpireAt - DateTime.UtcNow;
            if (lefttime < TimeSpan.Zero)
                lefttime = TimeSpan.Zero;
            return lefttime;
        }

        private async Task HubConnection_Reconnected(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
                return;

            await hubConnection.SendAsync("UpdateUserConnection", UserId);
        }

        private string GetMessageCounter() => $"{_messageInput?.Length ?? 0} / {AllowedLength}";

        async Task Send()
        {
            await hubConnection.SendAsync("SendMessage", UserId, MessageInput);
            MessageInput = string.Empty;
        }

        private bool IsSendBtnEnabled => !string.IsNullOrEmpty(MessageInput) && IsConnected;

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
            _timer?.Dispose();
            _timer = null;
        }

        private async void CopyInviteLink()
        {
            CreateRequest request = new CreateRequest
            {
                RoomId = RoomId,
                UserId = UserId,
                IsPermanent = true
            };

            var serviceResult = await InvitationService.Create(request);
            if (!serviceResult.IsOk)
            {
                AddAutoClosingNotification(serviceResult.ErrorMsg, "error", "cancel");
                return;
            }

            Guid invitationId = serviceResult.Result.InvitationId;
            string inviteLink = NavigationManager.ToAbsoluteUri($"/invite/{invitationId}").AbsoluteUri;
            await ClipboardService.WriteTextAsync(inviteLink);
            AddAutoClosingNotification("Invite link was copied", "success", "clipboard");
        }

        private void ShowUserList()
        {

        }

        private void AddAutoClosingNotification(string text, string theme, string icon)
        {
            NotificationReference.Show(new NotificationModel()
            {
                Text = text,
                ThemeColor = theme,
                Closable = false,
                CloseAfter = 2000,
                Icon = icon
            });
        }

        //void ValueChangedHandler(string input)
        //{
        //    TheValue = MaximumLength(input, MaxLength);
        //}

        //public string MaximumLength(string value, int maxLength)
        //{
        //    if (value.Length <= maxLength)
        //    {
        //        return value;
        //    }
        //    else
        //    {
        //        //WarningMessage = $"The maximum length of the string should be {maxLength}";
        //        throw new Exception("My exception");
        //    }
        //}
    }
}
