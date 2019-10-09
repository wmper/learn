﻿using DotNetCore.CAP;
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
            using (var connection = new MySqlConnection("Server=39.108.11.143;Port=33306;Database=cap;Uid=cherry;Pwd=cherry123456!@#;"))
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