using Example.Config.Hubs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

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
            //_hubContext.Clients.All.ReceiveMessage("ok");
        }
    }
}
