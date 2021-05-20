using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShareCode.Client.Components
{
    public partial class Chat : IAsyncDisposable
    {
        private class Message
        {
            public string Sender { get; set; }
            public string Text { get; set; }
            public DateTime Date { get; set; }
        }

        [Parameter]
        public Guid UserId { get; set; }

        [Parameter]
        public Guid RoomId { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;
        private ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        private string UserInput = "Boris";

        private int UsersCount => 0;

        private string _messageInput;
        private string MessageInput 
        { 
            get => _messageInput;
            set
            {
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
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
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
        }

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }

        private void CopyInviteLink()
        {

        }

        private void ShowUserList()
        {

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
