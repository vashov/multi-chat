using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MultiChat.Client.Services.Clipboard;
using MultiChat.Client.Services.Invitations;
using MultiChat.Shared;
using MultiChat.Shared.Helpers;
using MultiChat.Shared.Invitations.Create;
using MultiChat.Shared.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Telerik.Blazor.Components;

namespace MultiChat.Client.Components
{
    public partial class Chat : IAsyncDisposable
    {
        private class Message
        {
            public string UserName { get; set; }
            public Guid UserPublicId { get; set; }
            public string UserColor { get; set; }
            public string Text { get; set; }
            public DateTime Date { get; set; }
            public SendMessage.MessageTypeEnum MessageType { get; set; }

            public Message PrevMessage { get; set; }
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

        [Parameter]
        public Action<Guid> CloseChat { get; set; }

        public bool CanInvite => !OnlyOwnerCanInvite || UserPublicId == RoomOwnerPublicId;

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ClipboardService ClipboardService { get; set; }

        [Inject]
        private IInvitationService InvitationService { get; set; }

        [Inject] 
        private IJSRuntime JS { get; set; }


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

        private HubConnection _hubConnection;
        private ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        private int UsersCount => 0;

        private string _messageInput;
        private string MessageInput 
        { 
            get => _messageInput;
            set
            {
                value = value?.Replace("\n", null);
                int len = (value?.Length ?? 0) < _allowedLength ? value?.Length ?? 0 : _allowedLength;
                _messageInput = value?.Substring(0, len);
            }
        }

        private const int _allowedLength = 128;

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine($"Chat OnInitialized {RoomId}");

            InitializeTimer();
            await InitializeHubConnection();
        }

        private async Task InitializeHubConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri($"/hub/chat?userId={UserId}"))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.Reconnected += HubConnection_Reconnected;

            _hubConnection.On<SendMessage>("ReceiveMessage", HandleReceiveMessage);


            await _hubConnection.StartAsync();
        }

        private void InitializeTimer()
        {
            TimerDisplay = GetLeftTime().ToString(@"hh\:mm\:ss");

            _timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += UpdateTimer;
            _timer.Start();
        }

        private void UpdateTimer(object sender, ElapsedEventArgs e)
        {
            TimeSpan lefttime = GetLeftTime();
            TimerDisplay = lefttime.ToString(@"hh\:mm\:ss");

            if (lefttime == TimeSpan.Zero)
            {
                CloseChat.Invoke(RoomId);
            }
        }

        private async void HandleReceiveMessage(SendMessage message)
        {
            string color = ColorHelper.GetColor((ColorEnum)message.UserColor);
            var messageInfo = new Message
            {
                UserColor = color,
                UserName = message.UserName,
                UserPublicId = message.UserPublicId,
                Text = message.Text,
                Date = message.Date,
                MessageType = message.MessageType
            };

            if (Messages.Any())
            {
                var lastMessage = Messages.Last();
                messageInfo.PrevMessage = lastMessage;
            }

            Messages.Add(messageInfo);
            StateHasChanged();
            await Scroll();
        }

        private async Task Scroll()
        {
            await Task.Delay(10); // wait while message will be added to dom
            await JS.InvokeVoidAsync("ScrollChat", ListViewId);
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

            await _hubConnection.SendAsync("UpdateUserConnection", UserId);
        }

        private string GetMessageCounter() => $"{_messageInput?.Length ?? 0} / {_allowedLength}";

        async Task Send()
        {
            await _hubConnection.SendAsync("SendMessage", UserId, MessageInput);
            MessageInput = string.Empty;
        }

        private bool IsSendBtnEnabled => !string.IsNullOrEmpty(MessageInput) && IsConnected;

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
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

        private async void HandleEnterOnTextAreaPressed(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && IsSendBtnEnabled)
            {
                await Send();
            }
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
    }
}
