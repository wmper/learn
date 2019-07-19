using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Example.Consul.ServiceA.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var ip = NetworkInterface.GetAllNetworkInterfaces()
                                     .Select(p => p.GetIPProperties())
                                     .SelectMany(p => p.UnicastAddresses)
                                     .Where(p => p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address)).FirstOrDefault()?.Address.ToString();

            return new string[] { "ServiceA", ip };
        }
    }
}
