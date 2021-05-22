using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiChat.Client.Services.Clipboard;
using MultiChat.Client.Services.Invitations;
using MultiChat.Client.Services.Rooms;
using MultiChat.Client.Services.RoomsManager;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiChat.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IInvitationService, InvitationService>();

            builder.Services.AddScoped<ClipboardService>();
            builder.Services.AddSingleton<RoomsManagerService>();

            builder.Services.AddTelerikBlazor();

            await builder.Build().RunAsync();
        }
    }
}
