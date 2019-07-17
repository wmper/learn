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
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace ApiGateway
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOcelot(Configuration).AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //var serviceRegistration = new AgentServiceRegistration()
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    Name = "ApiGateWay",
            //    Address = "localhost",
            //    Port = 7000,
            //    Check = new AgentServiceCheck()
            //    {
            //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
            //        Interval = TimeSpan.FromSeconds(10),
            //        TCP = "http://localhost:7000",
            //        Timeout = TimeSpan.FromSeconds(5)
            //    }
            //};

            //ConsulClient client = new ConsulClient(x =>
            //{
            //    x.Address = new Uri("http://localhost:8500");
            //});

            //client.Agent.ServiceRegister(serviceRegistration).Wait();

            //applicationLifetime.ApplicationStopping.Register(() =>
            //{
            //    client.Agent.ServiceDeregister(serviceRegistration.ID).Wait();
            //});

            app.UseOcelot().Wait();
            //app.UseMvc();
        }
    }
}
