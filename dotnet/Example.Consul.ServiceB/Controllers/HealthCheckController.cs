using Microsoft.AspNetCore.Mvc;

namespace Example.Consul.ServiceB.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "ServiceB is OK";
        }
    }
}
