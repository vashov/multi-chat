using Telerik.Blazor.Components;

namespace MultiChat.Client.Services.Notify
{
    public class GlobalNotifyService
    {
        public TelerikNotification GlobalNotificationReference { get; internal set; }

        public void AddAutoClosingErrorNotification(string text)
        {
            GlobalNotificationReference.Show(new NotificationModel()
            {
                Text = text,
                ThemeColor = "error",
                Closable = false,
                CloseAfter = 2000,
                Icon = "exclamation-circle"
            });
        }
    }
}
