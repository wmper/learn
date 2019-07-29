using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Example.Config.Hubs
{
    public class ConfigHub : Hub<IClient>
    {
        public Task Init(string environmentName)
        {
            var obj = new { Consul = new { Key1 = "value1", Key2 = "value2" } };

            return Clients.Caller.ReceiveMessage(JsonConvert.SerializeObject(obj));
        }

        public override Task OnConnectedAsync()
        {
            if (Context.GetHttpContext().Request.Headers.TryGetValue("EnvironmentName", out StringValues value))
            {
                var environmentName = value.ToString();
                Groups.AddToGroupAsync(Context.ConnectionId, environmentName).GetAwaiter();
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.GetHttpContext().Request.Headers.TryGetValue("EnvironmentName", out StringValues value))
            {
                var environmentName = value.ToString();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, environmentName).GetAwaiter();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
