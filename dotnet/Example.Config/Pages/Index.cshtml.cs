using Example.Config.Hubs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;

namespace Example.Config.Pages
{
    public class IndexModel : PageModel
    {
        public IHubContext<ConfigHub, IClient> _hubContext { get; }

        public IndexModel(IHubContext<ConfigHub, IClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public void OnGet()
        {
            var obj = new { Consul = new { Key1 = "value1", Key2 = "value2" } };

            _hubContext.Clients.All.ReceiveMessage(JsonConvert.SerializeObject(obj));
        }
    }
}
