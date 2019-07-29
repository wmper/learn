using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace Example.Config.ClientA
{
    public static class CustSignalExtensions
    {
        private static string _version = string.Empty;
        private static IDictionary<string, object> _keyValuePairs = new Dictionary<string, object>();

        public static IApplicationBuilder UseCustSignalR(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var applicationLifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            HubConnection conn = null;
            applicationLifetime.ApplicationStarted.Register(async () =>
            {
                conn = new HubConnectionBuilder().WithUrl("http://localhost:5000/confighub", config =>
                {
                    config.Headers = new Dictionary<string, string>
                                    {
                                        { "EnvironmentName", env.EnvironmentName }
                                    };
                }).Build();

                conn.Closed += async (error) =>
                {
                    while (conn.State == HubConnectionState.Disconnected)
                    {
                        await Task.Delay(new Random().Next(0, 5) * 1000);
                        try
                        {
                            await conn.StartAsync();
                        }
                        catch (Exception) { }
                    }
                };

                conn.On<string>("ReceiveMessage", (message) =>
                {
                    var dic = JsonConvert.DeserializeObject<IDictionary<string, object>>(message);

                    foreach (var item in dic)
                    {
                        if (_keyValuePairs.ContainsKey(item.Key))
                        {
                            _keyValuePairs[item.Key] = item.Value;
                        }
                        else
                        {
                            _keyValuePairs.Add(item.Key, item.Value);
                        }

                        var path = AppDomain.CurrentDomain.BaseDirectory + $"/Config/{env.EnvironmentName}/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        File.WriteAllText(path + $"{item.Key}.json", JsonConvert.SerializeObject(item.Value));
                    }
                });

                try
                {
                    await conn.StartAsync();

                    await conn.InvokeAsync("Init", env.EnvironmentName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            applicationLifetime.ApplicationStopped.Register(async () =>
            {
                if (conn != null && conn.State == HubConnectionState.Connected)
                {
                    await conn.DisposeAsync();
                }
            });

            return app;
        }
    }
}
