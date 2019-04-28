using Example.Quartz.Infrastructure.AutofacModule;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Example.Quartz
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    // To read launchSettings.json environment
                    configHost.AddEnvironmentVariables();
                    if (args != null)
                    {
                        configHost.AddCommandLine(args);
                    }
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    // 设置根目录
                    configApp.SetBasePath(Directory.GetCurrentDirectory());

                    // 根据环境变量读取配置
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<QuartzHostService>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
