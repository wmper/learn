using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace Example.Consul.ServiceB.Extensions
{
    public static class UseConsulExtensions
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConfiguration configuration)
        {
            var path = "/api/consul/healthcheck";

            var consulConfig = new ConsulConfig();
            configuration.GetSection("Consul").Bind(consulConfig);

            var serviceRegistration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulConfig.Service.Name,
                Address = consulConfig.Service.Address,
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
            public int Port { get; set; }
            public Service Service { get; set; }
        }

        class Service
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public int Port { get; set; }
        }
    }
}
