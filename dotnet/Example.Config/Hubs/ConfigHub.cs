using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Example.Config.Hubs
{
    public class ConfigHub : Hub<IClient>
    {
        //public Task SendMessage(string message)
        //{
        //    return Clients.All.ReceiveMessage(message);
        //}

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
