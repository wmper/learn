using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Example.Consul.ServiceA.Extensions
{
    public static class UseConsulExtensions
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConfiguration configuration)
        {
            var path = "/api/consul/healthcheck";
            var ip = NetworkInterface.GetAllNetworkInterfaces()
                                     .Select(p => p.GetIPProperties())
                                     .SelectMany(p => p.UnicastAddresses)
                                     .Where(p => p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))
                                     .FirstOrDefault()?.Address.ToString();

            var consulConfig = new ConsulConfig();
            configuration.GetSection("Consul").Bind(consulConfig);

            var address = consulConfig.Service.Address;
            if (string.IsNullOrWhiteSpace(address) || address == "127.0.0.1")
            {
                address = ip;
            }

            var serviceRegistration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulConfig.Service.Name,
                Address = address,
                Port = consulConfig.Service.Port
            };

            serviceRegistration.Check = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(10),
                HTTP = $"http://{serviceRegistration.Address}:{serviceRegistration.Port}{path}",
                Timeout = TimeSpan.FromSeconds(5)
            };

            var client = new ConsulClient(x =>
            {
                x.Address = new Uri($"{consulConfig.Address}:{consulConfig.Port}");
            });

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                client.Agent.ServiceRegister(serviceRegistration).Wait();
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                client.Agent.ServiceDeregister(serviceRegistration.ID).Wait();
            });

            app.Map(path, config =>
            {
                config.Run(async context =>
                {
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("OK"));
                });
            });

            app.Map("/api/consul/register", config =>
            {
                config.Run(async context =>
                {
                    client.Agent.ServiceRegister(serviceRegistration).Wait();
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("OK"));
                });
            });

            app.Map("/api/consul/deregister", config =>
            {
                config.Run(async context =>
                {
                    client.Agent.ServiceDeregister(serviceRegistration.ID).Wait();
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("OK"));
                });
            });

            return app;
        }

        class ConsulConfig
        {
            public string Address { get; set; }
            public int Port { get; set; } = 8500;
            public Service Service { get; set; }
        }

        class Service
        {
            public string Name { get; set; }
            public string Address { get; set; } = "127.0.0.1";
            public int Port { get; set; } = 80;
        }
    }
}
