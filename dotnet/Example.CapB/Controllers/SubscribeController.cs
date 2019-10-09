using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Example.CapB.Controllers
{
    public class SubscribeController : Controller
    {
        [CapSubscribe("xxx.services.show.time")]
        public void CheckReceivedMessage(DateTime datetime)
        {
            Console.WriteLine(datetime);
        }
    }
}
