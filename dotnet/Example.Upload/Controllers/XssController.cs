using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Upload.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Example.Upload.Controllers
{
    [Route("api/[controller]")]
    public class XssController : Controller
    {
        [HttpPost]
        [XSS]
        public string Post([FromBody] Test value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

    public class Test
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Second Second { get; set; }
    }

    public class Second
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }
}
