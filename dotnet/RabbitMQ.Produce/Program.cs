﻿using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Produce
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Produce Starting.");

            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "192.168.137.20",
                Port = 5672
            };

            var conn = factory.CreateConnection();

            var channel = conn.CreateModel();

            //channel.ConfirmSelect();

            channel.ExchangeDeclare(exchange: "test.exchange", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

            channel.QueueDeclare(queue: "test.queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "test.queue", exchange: "test.exchange", routingKey: "test.key", arguments: null);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            for (var i = 1; i <= 10; i++)
            {
                var msg = $"No:{i},hello world!time:-" + new Random().Next(1, 20);
                var body = Encoding.UTF8.GetBytes(msg);


                channel.BasicPublish(exchange: "test.exchange", routingKey: "test.key", basicProperties: properties, body: body);
                Console.WriteLine("send." + msg);

                //channel.WaitForConfirms();
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.Read();
        }
    }
}
