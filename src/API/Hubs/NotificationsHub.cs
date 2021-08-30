using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace powerplant_coding_challenge.Hubs
{
    public class NotificationsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
        }
    }
}
