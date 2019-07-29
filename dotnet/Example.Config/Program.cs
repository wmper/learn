﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Example.Config
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureLogging(logging =>
                    {
                        logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                        logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                    })
                   .UseStartup<Startup>();
    }
}
