using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Upload
{
    public class CPUUsageHostedService : IHostedService, IDisposable
    {
        private Timer _timer { get; set; }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            GetCPUUsage.OnStartup();

            _timer = new Timer(state =>
            {
                GetCPUUsage.CallCPU();

                Console.WriteLine(GetCPUInfo.GetInfoTotal());
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
