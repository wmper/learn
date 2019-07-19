﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Example.Consul.ServiceB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://localhost:9000")
                .UseStartup<Startup>();
    }
}
