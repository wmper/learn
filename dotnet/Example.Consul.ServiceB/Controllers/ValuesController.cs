using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Example.Consul.ServiceB.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "ServiceB" };
        }
    }
}
