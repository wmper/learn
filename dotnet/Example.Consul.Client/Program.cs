using Consul;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Example.Consul.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                    .Select(p => p.GetIPProperties())
                    .SelectMany(p => p.UnicastAddresses)
                    .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                    .FirstOrDefault()?.Address.ToString();


            Console.WriteLine(ip);

            ConsulClient consul = new ConsulClient(c =>
           {
               c.Address = new Uri("http://localhost:8500");
           });

            var services = consul.Catalog.Service("ApiService").Result.Response;
            if (services != null && services.Any())
            {
                // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                //while (true)
                //{
                    Random r = new Random();
                    int index = r.Next(services.Count());

                    var service = services.ElementAt(index);

                    using (HttpClient client = new HttpClient())
                    {
                        var response = client.GetStringAsync($"{service.ServiceAddress}:{service.ServicePort}/api/values");
                        Console.WriteLine(response.Result);
                    }

                    Thread.Sleep(500);
                //}
            }

            

            Console.ReadLine();
        }
    }
}
