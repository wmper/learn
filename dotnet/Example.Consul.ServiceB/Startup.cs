using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Example.Consul.ServiceB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            var serviceRegistration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "ApiService",
                Address = "localhost",
                Port = 9000,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = "http://localhost:9000/api/healthcheck",
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            ConsulClient client = new ConsulClient(x =>
            {
                x.Address = new Uri("http://localhost:8500");
            });

            client.Agent.ServiceRegister(serviceRegistration).Wait();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                client.Agent.ServiceDeregister(serviceRegistration.ID).Wait();
            });
        }
    }
}
