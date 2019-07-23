using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Example.Config.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var conn = new HubConnectionBuilder().WithUrl("http://localhost:5000/confighub").Build();

            conn.On<string>("ReceiveMessage", (message) =>
            {
                Console.WriteLine($"{message}");
            });

            try
            {
                await conn.StartAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Console.ReadLine();
        }
    }
}
