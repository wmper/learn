using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consume
{
    public class ConsumerHostedService : IHostedService
    {
        private IConnection _conn { get; set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost",
                Port = 5672
            };
            _conn = factory.CreateConnection();

            var channel = _conn.CreateModel();

            //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                
                // ... process the message
                var msg = Encoding.UTF8.GetString(body);
                Console.WriteLine(msg);

                //var s = msg.Split('-')[1].ToString();
                //Thread.Sleep(int.Parse(s) * 1000);

                //channel.BasicAck(ea.DeliveryTag, true);
            };

            var consumerTag = channel.BasicConsume(queue: "test.queue", autoAck: false, consumerTag: "consumer-test" + new Random().Next(1, 9), consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_conn.IsOpen)
            {
                _conn.Close();
            }

            return Task.CompletedTask;
        }
    }
}
