using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;

namespace Example.CapA.Controllers
{
    public class PublishController : Controller
    {
        private readonly ICapPublisher _capBus;

        public PublishController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        [HttpGet("/adonet/transaction")]
        public IActionResult AdonetWithTransaction()
        {
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Database=cap;Uid=root;Pwd=;"))
            {
                using (var transaction = connection.BeginTransaction(_capBus, autoCommit: true))
                {
                    //your business logic code

                    _capBus.Publish("xxx.services.show.time", DateTime.Now);
                }
            }

            return Ok();
        }
    }
}
