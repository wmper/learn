using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Quartz
{
    public class QuartzHostService : IHostedService
    {
        private readonly ILogger _log;
        private readonly IScheduler _scheduler;

        public QuartzHostService(ILogger<QuartzHostService> log, IScheduler scheduler)
        {
            _log = log;
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("QuartzHostService Starting...");

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("QuartzHostService Shutdown.");

            await _scheduler.Shutdown(cancellationToken);
        }
    }
}
