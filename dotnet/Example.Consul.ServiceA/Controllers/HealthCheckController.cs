using Microsoft.AspNetCore.Mvc;

namespace Example.Consul.ServiceA.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "ServiceA is OK";
        }
    }
}
